# Nightly benchmark run

<!-- nightly-baseline: 116c2b60eb01f66d877a72dad27d55a719473120 -->
<!-- nightly-owed: TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->
<!-- nightly-cut: TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance.md). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest.md).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `116c2b60eb01f66d877a72dad27d55a719473120`
- Previous run: `116c2b60eb01f66d877a72dad27d55a719473120`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `FilteredVectorSearchBenchmarks`
- `VectorStoreChurnBenchmarks`
- `SentencePieceBpeLineageBenchmarks`
- `BertNormalizerBenchmarks`
- `AddedTokenScanBenchmarks`
- `PrecompiledNormalizerBenchmarks`
- `EmbeddingSearchHelpersBenchmarks`
- `RankingMetricsBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `ClassifierCurveBenchmarks`
- `PartitionValidityBenchmarks`
- `ClassificationReportBenchmarks`
- `MultiClassRocAucBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `DecompositionWidthBenchmarks`
- `HouseholderQrBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `LeastSquaresRoutingBenchmarks`
- `BkTreeNearestBenchmarks`
- `CosineTopKSelectionBenchmarks`

### Lodestar.Gpu.Benchmarks.CosineTopKSelectionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Documents | K    | Device               | Mean          | Error         | StdDev       | Ratio    | RatioSD | Allocated   | Alloc Ratio |
|------------- |---------- |----- |--------------------- |--------------:|--------------:|-------------:|---------:|--------:|------------:|------------:|
| **SimdBaseline** | **10000**     | **10**   | **CPUAc(...) GPU) [26]** |      **10.63 ms** |      **0.640 ms** |     **0.035 ms** |     **1.00** |    **0.00** |    **26.02 KB** |        **1.00** |
| GpuResident  | 10000     | 10   | CPUAc(...) GPU) [26] |  21,889.58 ms |  1,961.586 ms |   107.521 ms | 2,058.84 |   10.56 |   1518.2 KB |       58.36 |
|              |           |      |                      |               |               |              |          |         |             |             |
| **SimdBaseline** | **10000**     | **100**  | **CPUAc(...) GPU) [26]** |      **10.12 ms** |      **1.249 ms** |     **0.068 ms** |     **1.00** |    **0.01** |    **37.27 KB** |        **1.00** |
| GpuResident  | 10000     | 100  | CPUAc(...) GPU) [26] |  22,773.43 ms |  7,872.603 ms |   431.524 ms | 2,250.95 |   39.22 |  1541.63 KB |       41.37 |
|              |           |      |                      |               |               |              |          |         |             |             |
| **SimdBaseline** | **10000**     | **1000** | **CPUAc(...) GPU) [26]** |      **14.31 ms** |      **0.426 ms** |     **0.023 ms** |     **1.00** |    **0.00** |   **149.77 KB** |        **1.00** |
| GpuResident  | 10000     | 1000 | CPUAc(...) GPU) [26] |  26,375.52 ms | 15,849.815 ms |   868.782 ms | 1,842.62 |   52.63 |  1765.98 KB |       11.79 |
|              |           |      |                      |               |               |              |          |         |             |             |
| **SimdBaseline** | **100000**    | **10**   | **CPUAc(...) GPU) [26]** |     **274.42 ms** |     **30.405 ms** |     **1.667 ms** |     **1.00** |    **0.01** |    **26.36 KB** |        **1.00** |
| GpuResident  | 100000    | 10   | CPUAc(...) GPU) [26] | 220,459.85 ms | 37,648.257 ms | 2,063.628 ms |   803.38 |    7.76 | 39277.32 KB |    1,490.07 |
|              |           |      |                      |               |               |              |          |         |             |             |
| **SimdBaseline** | **100000**    | **100**  | **CPUAc(...) GPU) [26]** |     **286.96 ms** |     **73.694 ms** |     **4.039 ms** |     **1.00** |    **0.02** |    **37.61 KB** |        **1.00** |
| GpuResident  | 100000    | 100  | CPUAc(...) GPU) [26] | 219,513.07 ms | 45,360.450 ms | 2,486.359 ms |   765.07 |   12.02 | 39300.48 KB |    1,044.96 |
|              |           |      |                      |               |               |              |          |         |             |             |
| **SimdBaseline** | **100000**    | **1000** | **CPUAc(...) GPU) [26]** |     **301.56 ms** |     **46.642 ms** |     **2.557 ms** |     **1.00** |    **0.01** |   **150.11 KB** |        **1.00** |
| GpuResident  | 100000    | 1000 | CPUAc(...) GPU) [26] | 221,525.62 ms | 47,910.777 ms | 2,626.151 ms |   734.64 |    9.26 | 39525.76 KB |      263.31 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.GlmNegativeBinomialBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       |    **207.6 μs** |    **44.97 μs** |   **2.46 μs** |   **0.4883** |        **-** |        **-** |    **58.2 KB** |
| **Fit**    | **10000**      |  **2,570.5 μs** |   **423.01 μs** |  **23.19 μs** |  **97.6563** |  **97.6563** |  **97.6563** |  **550.45 KB** |
| **Fit**    | **100000**     | **26,605.9 μs** | **8,147.65 μs** | **446.60 μs** | **437.5000** | **437.5000** | **437.5000** | **5473.82 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.KolmogorovDurbinBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method     | SampleSize | Mean         | Error       | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|----------- |----------- |-------------:|------------:|----------:|--------:|--------:|--------:|----------:|
| **Asymptotic** | **100**        |     **7.773 μs** |   **0.3981 μs** | **0.0218 μs** |  **0.0458** |       **-** |       **-** |   **4.73 KB** |
| **Asymptotic** | **280**        |   **120.895 μs** |  **15.1288 μs** | **0.8293 μs** |  **0.2441** |       **-** |       **-** |   **24.7 KB** |
| **Asymptotic** | **20000**      | **1,176.126 μs** | **111.3224 μs** | **6.1020 μs** | **99.6094** | **99.6094** | **99.6094** | **352.77 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Stats.Benchmarks.LeastSquaresRoutingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method            | Regressors | Mean         | Error         | StdDev      | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated   | Alloc Ratio |
|------------------ |----------- |-------------:|--------------:|------------:|------:|--------:|---------:|---------:|---------:|------------:|------------:|
| **Lodestar_Estimate** | **4**          |     **110.2 μs** |      **39.56 μs** |     **2.17 μs** |  **1.00** |    **0.02** |  **49.9268** |  **49.9268** |  **49.9268** |   **188.26 KB** |        **1.00** |
| Lodestar_Fit      | 4          |     182.4 μs |       5.18 μs |     0.28 μs |  1.65 |    0.03 |   0.2441 |        - |        - |    33.12 KB |        0.18 |
| MathNet_Qr        | 4          |     576.0 μs |     321.17 μs |    17.60 μs |  5.23 |    0.16 | 190.4297 | 190.4297 | 190.4297 |   705.13 KB |        3.75 |
|                   |            |              |               |             |       |         |          |          |          |             |             |
| **Lodestar_Estimate** | **250**        |  **79,403.3 μs** |  **39,876.07 μs** | **2,185.74 μs** |  **1.00** |    **0.03** | **428.5714** | **428.5714** | **428.5714** |  **8865.83 KB** |        **1.00** |
| Lodestar_Fit      | 250        | 310,427.8 μs |  68,082.47 μs | 3,731.83 μs |  3.91 |    0.10 |        - |        - |        - |  3499.66 KB |        0.39 |
| MathNet_Qr        | 250        | 499,210.3 μs | 176,831.78 μs | 9,692.75 μs |  6.29 |    0.19 |        - |        - |        - | 32672.16 KB |        3.69 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.AddedTokenScanBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                  | Mean     | Error     | StdDev   | Gen0     | Allocated |
|------------------------ |---------:|----------:|---------:|---------:|----------:|
| ChatTemplate            | 95.97 ms |  8.396 ms | 0.460 ms | 333.3333 |   33.3 MB |
| ProseWithoutAddedTokens | 79.84 ms | 13.983 ms | 0.766 ms | 285.7143 |  28.47 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BertNormalizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Text     | Lowercase | Mean     | Error     | StdDev   | Gen0     | Allocated |
|------- |--------- |---------- |---------:|----------:|---------:|---------:|----------:|
| **Encode** | **Ascii**    | **False**     | **31.09 ms** |  **0.701 ms** | **0.038 ms** | **125.0000** |   **11.7 MB** |
| **Encode** | **Ascii**    | **True**      | **34.94 ms** |  **4.173 ms** | **0.229 ms** | **133.3333** |   **11.7 MB** |
| **Encode** | **Accented** | **False**     | **30.87 ms** |  **0.623 ms** | **0.034 ms** | **125.0000** |  **11.69 MB** |
| **Encode** | **Accented** | **True**      | **44.38 ms** |  **4.787 ms** | **0.262 ms** | **250.0000** |  **20.38 MB** |
| **Encode** | **Cjk**      | **False**     | **35.93 ms** | **10.049 ms** | **0.551 ms** | **266.6667** |  **21.55 MB** |
| **Encode** | **Cjk**      | **True**      | **48.99 ms** |  **7.198 ms** | **0.395 ms** | **272.7273** |  **24.48 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BkTreeNearestBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Count | Mean     | Error     | StdDev   | Ratio | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------ |------ |---------:|----------:|---------:|------:|----------:|----------:|----------:|------------:|------------:|
| **FullScan**    | **10**    | **79.49 ms** | **12.872 ms** | **0.706 ms** |  **1.00** | **1857.1429** | **1857.1429** | **1857.1429** |  **9386.09 KB** |       **1.000** |
| TreeNearest | 10    | 51.44 ms |  1.420 ms | 0.078 ms |  0.65 |         - |         - |         - |    29.67 KB |       0.003 |
|             |       |          |           |          |       |           |           |           |             |             |
| **FullScan**    | **2000**  | **81.81 ms** |  **5.709 ms** | **0.313 ms** |  **1.00** | **1857.1429** | **1857.1429** | **1857.1429** | **10007.93 KB** |        **1.00** |
| TreeNearest | 2000  | 82.82 ms |  3.899 ms | 0.214 ms |  1.01 |         - |         - |         - |  1273.52 KB |        0.13 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassificationReportBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | Classes | Mean         | Error        | StdDev      | Gen0   | Allocated |
|------- |-------- |-------------:|-------------:|------------:|-------:|----------:|
| **Report** | **10**      |     **516.7 ns** |     **36.30 ns** |     **1.99 ns** | **0.0200** |   **1.66 KB** |
| **Report** | **1000**    | **310,012.8 ns** | **21,429.61 ns** | **1,174.63 ns** | **1.4648** | **121.55 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.ClassifierCurveBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Samples | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|---------------- |-------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| Roc             | 1000000 | 138.1 ms | 33.03 ms | 1.81 ms |  750.0000 |  750.0000 |  750.0000 |  53.79 MB |
| PrecisionRecall | 1000000 | 125.6 ms | 25.78 ms | 1.41 ms | 1000.0000 | 1000.0000 | 1000.0000 |  68.66 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionWidthBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                     | Columns | Mean         | Error         | StdDev      | Gen0      | Gen1      | Gen2      | Allocated    |
|--------------------------- |-------- |-------------:|--------------:|------------:|----------:|----------:|----------:|-------------:|
| **TruncatedSvd_Rank20**        | **500**     |  **19,732.5 μs** |   **4,278.16 μs** |   **234.50 μs** | **3000.0000** | **3000.0000** | **3000.0000** |  **11692.44 KB** |
| TruncatedSvd_Transform     | 500     |     529.6 μs |      33.77 μs |     1.85 μs |   90.8203 |   90.8203 |   90.8203 |    312.58 KB |
| Nmf_Frobenius_Rank20       | 500     | 183,847.5 μs | 111,613.03 μs | 6,117.89 μs | 4500.0000 | 4500.0000 | 4500.0000 |  18141.12 KB |
| Nmf_KullbackLeibler_Rank20 | 500     | 204,194.7 μs |  62,079.69 μs | 3,402.80 μs | 4000.0000 | 4000.0000 | 4000.0000 |  18596.18 KB |
| **TruncatedSvd_Rank20**        | **20000**   | **170,230.1 μs** |  **28,277.98 μs** | **1,550.01 μs** | **3333.3333** | **3333.3333** | **3333.3333** | **115064.01 KB** |
| TruncatedSvd_Transform     | 20000   |     908.0 μs |     119.36 μs |     6.54 μs |   43.9453 |   43.9453 |   43.9453 |    312.55 KB |
| Nmf_Frobenius_Rank20       | 20000   | 781,012.0 μs |  65,690.49 μs | 3,600.72 μs | 5000.0000 | 5000.0000 | 5000.0000 | 166225.81 KB |
| Nmf_KullbackLeibler_Rank20 | 20000   | 559,905.3 μs |  62,451.92 μs | 3,423.20 μs | 5000.0000 | 5000.0000 | 5000.0000 |  166680.8 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.EmbeddingSearchHelpersBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | Rows   | Mean        | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|-------------------- |------- |------------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| **FromBlockNormalized** | **1000**   |    **703.8 μs** |   **124.1 μs** |   **6.80 μs** | **249.0234** | **249.0234** | **249.0234** |  **1500.18 KB** |
| MmrSelect100        | 1000   |  6,299.2 μs |   513.3 μs |  28.13 μs |        - |        - |        - |    24.93 KB |
| **FromBlockNormalized** | **100000** | **79,968.9 μs** | **7,927.3 μs** | **434.52 μs** | **166.6667** | **166.6667** | **166.6667** | **150000.3 KB** |
| MmrSelect100        | 100000 |  6,068.8 μs |   617.1 μs |  33.82 μs |        - |        - |        - |    24.93 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.FilteredVectorSearchBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Records | Mean        | Error        | StdDev      | Gen0   | Allocated  |
|---------------- |-------- |------------:|-------------:|------------:|-------:|-----------:|
| **FilteredTop10**   | **10000**   |    **528.8 μs** |     **59.55 μs** |     **3.26 μs** |      **-** |    **7.17 KB** |
| Unfiltered      | 10000   |    609.9 μs |     15.29 μs |     0.84 μs |      - |    2.93 KB |
| HybridSelective | 10000   |    682.9 μs |     47.29 μs |     2.59 μs |      - |   15.11 KB |
| HybridBroad     | 10000   |  1,540.6 μs |  1,162.60 μs |    63.73 μs | 1.9531 |  342.49 KB |
| **FilteredTop10**   | **100000**  |  **8,840.5 μs** | **18,528.64 μs** | **1,015.62 μs** |      **-** |    **7.18 KB** |
| Unfiltered      | 100000  | 16,043.5 μs | 21,970.27 μs | 1,204.26 μs |      - |    2.95 KB |
| HybridSelective | 100000  | 16,524.2 μs |  5,732.74 μs |   314.23 μs |      - |   15.16 KB |
| HybridBroad     | 100000  | 25,926.9 μs |  1,139.85 μs |    62.48 μs |      - | 3155.13 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.HouseholderQrBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Rows  | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |------ |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| **Householder** | **2000**  |  **1.503 ms** | **0.9667 ms** | **0.0530 ms** | **253.9063** | **251.9531** | **250.0000** |   **1.38 MB** |
| **Householder** | **20000** | **25.289 ms** | **3.5004 ms** | **0.1919 ms** | **968.7500** | **968.7500** | **968.7500** |  **13.74 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultiClassRocAucBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method    | Samples | Mean        | Error      | StdDev    | Allocated |
|---------- |-------- |------------:|-----------:|----------:|----------:|
| **OneVsRest** | **100000**  |    **40.29 ms** |   **2.225 ms** |  **0.122 ms** |         **-** |
| OneVsOne  | 100000  |    81.25 ms |  11.097 ms |  0.608 ms |  803417 B |
| **OneVsRest** | **1000000** |   **786.96 ms** | **491.069 ms** | **26.917 ms** |         **-** |
| OneVsOne  | 1000000 | 1,331.57 ms | 222.724 ms | 12.208 ms | 8005264 B |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.MultilabelConfusionMatrixBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method           | Rows   | Mean      | Error      | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------- |------- |----------:|-----------:|----------:|---------:|---------:|---------:|------------:|
| PerLabel         | 100000 |  7.582 ms |  0.6259 ms | 0.0343 ms |        - |        - |        - |     7.85 KB |
| PerLabelWeighted | 100000 | 10.720 ms |  3.3214 ms | 0.1821 ms |        - |        - |        - |     7.85 KB |
| PerSample        | 100000 | 82.249 ms | 88.4704 ms | 4.8494 ms | 571.4286 | 428.5714 | 142.8571 | 39062.72 KB |
| PerClass         | 100000 |  4.327 ms |  0.3258 ms | 0.0179 ms |        - |        - |        - |     7.99 KB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PartitionValidityBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                | Samples | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|---------------------- |-------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| DaviesBouldinScore    | 1000000 |  8.345 ms | 0.9436 ms | 0.0517 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |
| CalinskiHarabaszScore | 1000000 | 13.562 ms | 2.7631 ms | 0.1515 ms | 140.6250 | 140.6250 | 140.6250 |   3.82 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PrecompiledNormalizerBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | Mean     | Error    | StdDev   | Gen0     | Allocated |
|------------------- |---------:|---------:|---------:|---------:|----------:|
| NormalizeDocuments | 12.24 ms | 0.460 ms | 0.025 ms |  31.2500 |   2.75 MB |
| EncodeDocuments    | 55.25 ms | 3.057 ms | 0.168 ms | 100.0000 |   8.19 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.RankingMetricsBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                            | Rows   | Mean      | Error     | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|---------------------------------- |------- |----------:|----------:|----------:|--------:|--------:|--------:|----------:|
| NdcgTieAveraged                   | 100000 | 32.125 ms | 3.5679 ms | 0.1956 ms |       - |       - |       - |  800342 B |
| NdcgIgnoringTies                  | 100000 | 29.777 ms | 0.2947 ms | 0.0162 ms |       - |       - |       - |  800319 B |
| DcgTieAveraged                    | 100000 | 18.417 ms | 0.3234 ms | 0.0177 ms |       - |       - |       - |  800319 B |
| ReciprocalRankScore               | 100000 | 15.206 ms | 0.7361 ms | 0.0404 ms |       - |       - |       - |     180 B |
| CoverageErrorScore                | 100000 |  6.578 ms | 0.2187 ms | 0.0120 ms | 15.6250 | 15.6250 | 15.6250 |  800040 B |
| LabelRankingAveragePrecisionScore | 100000 | 54.212 ms | 4.1338 ms | 0.2266 ms |       - |       - |       - |         - |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.SentencePieceBpeLineageBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method          | Model   | Mean     | Error   | StdDev  | Gen0     | Allocated |
|---------------- |-------- |---------:|--------:|--------:|---------:|----------:|
| **EncodeDocuments** | **Llama2**  | **143.9 ms** | **6.45 ms** | **0.35 ms** | **250.0000** |  **37.05 MB** |
| **EncodeDocuments** | **Mistral** | **138.9 ms** | **2.95 ms** | **0.16 ms** | **250.0000** |  **37.03 MB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.VectorStoreChurnBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]     : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-HGOFTC : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun   : .NET 10.0.14 (10.0.1426.52326), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

LaunchCount=1
```

<!-- markdownlint-disable MD060 -->

| Method           | Job        | IterationCount | RunStrategy | WarmupCount | Records | Mean         | Error      | StdDev     | Gen0   | Allocated |
|----------------- |----------- |--------------- |------------ |------------ |-------- |-------------:|-----------:|-----------:|-------:|----------:|
| **UpsertThenSearch** | **Job-HGOFTC** | **20**             | **Throughput**  | **5**           | **1000**    |     **62.22 μs** |   **1.377 μs** |   **1.586 μs** |      **-** |   **3.15 KB** |
| UpsertThenHybrid | Job-HGOFTC | 20             | Throughput  | 5           | 1000    |     84.42 μs |   0.950 μs |   1.094 μs | 0.2441 |  20.58 KB |
| Hybrid           | Job-HGOFTC | 20             | Throughput  | 5           | 1000    |     72.99 μs |   0.850 μs |   0.978 μs | 0.1221 |  15.52 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 1000    |     65.91 μs |   9.134 μs |   0.501 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 1000    |     81.38 μs |  31.265 μs |   1.714 μs | 0.2441 |  20.58 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 1000    |     72.89 μs |  20.601 μs |   1.129 μs | 0.1221 |  15.52 KB |
| **UpsertThenSearch** | **Job-HGOFTC** | **20**             | **Throughput**  | **5**           | **10000**   |    **700.18 μs** |   **2.826 μs** |   **3.141 μs** |      **-** |   **3.15 KB** |
| UpsertThenHybrid | Job-HGOFTC | 20             | Throughput  | 5           | 10000   |  1,000.33 μs |   9.425 μs |   9.256 μs |      - |  26.17 KB |
| Hybrid           | Job-HGOFTC | 20             | Throughput  | 5           | 10000   |    957.22 μs |  11.123 μs |  12.809 μs |      - |  21.14 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 10000   |    616.61 μs |  95.114 μs |   5.214 μs |      - |   3.15 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 10000   |  1,094.44 μs |  44.666 μs |   2.448 μs |      - |  26.17 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 10000   |    948.42 μs | 101.213 μs |   5.548 μs |      - |  21.14 KB |
| **UpsertThenSearch** | **Job-HGOFTC** | **20**             | **Throughput**  | **5**           | **100000**  | **16,403.14 μs** | **333.482 μs** | **342.461 μs** |      **-** |   **3.23 KB** |
| UpsertThenHybrid | Job-HGOFTC | 20             | Throughput  | 5           | 100000  | 21,800.91 μs | 119.228 μs | 137.303 μs |      - |  45.96 KB |
| Hybrid           | Job-HGOFTC | 20             | Throughput  | 5           | 100000  | 21,941.85 μs | 110.316 μs | 122.616 μs |      - |  41.06 KB |
| UpsertThenSearch | ShortRun   | 3              | Default     | 3           | 100000  | 16,659.77 μs | 765.179 μs |  41.942 μs |      - |   3.22 KB |
| UpsertThenHybrid | ShortRun   | 3              | Default     | 3           | 100000  | 21,483.18 μs | 327.570 μs |  17.955 μs |      - |  46.03 KB |
| Hybrid           | ShortRun   | 3              | Default     | 3           | 100000  | 21,158.80 μs | 564.834 μs |  30.960 μs |      - |  41.05 KB |

<!-- markdownlint-enable MD060 -->

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `TiledCosineTopKBenchmarks`
- `TiledSparseDenseProductBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

No ratio stepped past its noise or drifted over ten days tonight.
