# Nightly benchmark run

<!-- nightly-baseline: 724c791262e7139428ab02fd4e70a6e05305bac7 -->
<!-- nightly-owed: AddedTokenScanBenchmarks BertNormalizerBenchmarks BkTreeNearestBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks ClusteringAgreementBenchmarks CosineTopKSelectionBenchmarks DamerauLevenshteinBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchBenchmarks EmbeddingSearchHelpersBenchmarks EncoderIncumbentBenchmarks FilteredVectorSearchBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MathNetInteropBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks OsaBenchmarks PartitionValidityBenchmarks PrecompiledNormalizerBenchmarks ProcessExtractBenchmarks QgramBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RatcliffObershelpBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks ScalerIncumbentBenchmarks SentencePieceBpeLineageBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks SplitterIncumbentBenchmarks StationarityBenchmarks StatsBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TopKAccuracyBenchmarks TransformerIncumbentBenchmarks VectorAutoregressionBenchmarks VectorStoreChurnBenchmarks WeightedLeastSquaresBenchmarks -->
<!-- nightly-cut: CosineTopKSelectionBenchmarks TiledCosineTopKBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance.md). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest.md).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `724c791262e7139428ab02fd4e70a6e05305bac7`
- Previous run: `724c791262e7139428ab02fd4e70a6e05305bac7`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `TiledSparseDenseProductBenchmarks`
- `BitParallelEditDistanceBenchmarks`
- `ChainedProductBenchmarks`

### Lodestar.Gpu.Benchmarks.BitParallelEditDistanceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Texts  | TextLength | Device               | Mean       | Error       | StdDev     | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated  | Alloc Ratio |
|------------ |------- |----------- |--------------------- |-----------:|------------:|-----------:|------:|--------:|---------:|---------:|---------:|-----------:|------------:|
| **CpuBaseline** | **10000**  | **32**         | **CPUAc(...) GPU) [26]** |   **2.599 ms** |   **0.0188 ms** |  **0.0010 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |        **4 B** |        **1.00** |
| GpuResident | 10000  | 32         | CPUAc(...) GPU) [26] |   5.034 ms |   9.5171 ms |  0.5217 ms |  1.94 |    0.17 |        - |        - |        - |    42832 B |   10,708.00 |
| GpuFromHost | 10000  | 32         | CPUAc(...) GPU) [26] |   5.581 ms |  20.5345 ms |  1.1256 ms |  2.15 |    0.38 |  93.7500 |  93.7500 |  93.7500 |   471059 B |  117,764.75 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **10000**  | **256**        | **CPUAc(...) GPU) [26]** |  **12.099 ms** |   **0.0882 ms** |  **0.0048 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |       **16 B** |        **1.00** |
| GpuResident | 10000  | 256        | CPUAc(...) GPU) [26] |   9.266 ms |  19.2470 ms |  1.0550 ms |  0.77 |    0.08 |        - |        - |        - |    42840 B |    2,677.50 |
| GpuFromHost | 10000  | 256        | CPUAc(...) GPU) [26] |  11.473 ms |  10.0008 ms |  0.5482 ms |  0.95 |    0.04 | 328.1250 | 328.1250 | 328.1250 |  2711059 B |  169,441.19 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **32**         | **CPUAc(...) GPU) [26]** |  **51.284 ms** |   **0.4519 ms** |  **0.0248 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **102 B** |        **1.00** |
| GpuResident | 200000 | 32         | CPUAc(...) GPU) [26] |  42.680 ms |  21.2882 ms |  1.1669 ms |  0.83 |    0.02 |        - |        - |        - |   802891 B |    7,871.48 |
| GpuFromHost | 200000 | 32         | CPUAc(...) GPU) [26] |  60.410 ms | 293.5430 ms | 16.0901 ms |  1.18 |    0.27 | 375.0000 | 375.0000 | 375.0000 |  8071921 B |   79,136.48 |
|             |        |            |                      |            |             |            |       |         |          |          |          |            |             |
| **CpuBaseline** | **200000** | **256**        | **CPUAc(...) GPU) [26]** | **242.980 ms** |   **1.9698 ms** |  **0.1080 ms** |  **1.00** |    **0.00** |        **-** |        **-** |        **-** |      **341 B** |        **1.00** |
| GpuResident | 200000 | 256        | CPUAc(...) GPU) [26] | 141.782 ms |  59.7071 ms |  3.2727 ms |  0.58 |    0.01 |        - |        - |        - |   803080 B |    2,355.07 |
| GpuFromHost | 200000 | 256        | CPUAc(...) GPU) [26] | 196.560 ms |  48.9818 ms |  2.6849 ms |  0.81 |    0.01 | 333.3333 | 333.3333 | 333.3333 | 52871293 B |  155,047.78 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.ChainedProductBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Rows  | Width | Device               | Mean          | Error         | StdDev      | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|------------- |------ |------ |--------------------- |--------------:|--------------:|------------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| **RoundTripped** | **2000**  | **32**    | **CPUAc(...) GPU) [26]** |  **1,833.506 ms** |   **322.6532 ms** |  **17.6857 ms** | **1.000** |    **0.01** |          **-** |          **-** |          **-** |   **7.32 MB** |        **1.00** |
| Chained      | 2000  | 32    | CPUAc(...) GPU) [26] |  1,785.777 ms | 1,150.7003 ms |  63.0738 ms | 0.974 |    0.03 |          - |          - |          - |   5.36 MB |        0.73 |
| CpuBaseline  | 2000  | 32    | CPUAc(...) GPU) [26] |      1.582 ms |     1.0514 ms |   0.0576 ms | 0.001 |    0.00 |   330.0781 |   330.0781 |   330.0781 |   1.47 MB |        0.20 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **2000**  | **128**   | **CPUAc(...) GPU) [26]** |  **7,201.796 ms** | **1,851.2048 ms** | **101.4708 ms** | **1.000** |    **0.02** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **36.88 MB** |        **1.00** |
| Chained      | 2000  | 128   | CPUAc(...) GPU) [26] |  7,318.144 ms | 2,547.7987 ms | 139.6534 ms | 1.016 |    0.02 |  1000.0000 |  1000.0000 |  1000.0000 |  35.38 MB |        0.96 |
| CpuBaseline  | 2000  | 128   | CPUAc(...) GPU) [26] |      6.098 ms |     1.5072 ms |   0.0826 ms | 0.001 |    0.00 |   492.1875 |   492.1875 |   492.1875 |   5.86 MB |        0.16 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **32**    | **CPUAc(...) GPU) [26]** |  **7,356.624 ms** | **1,534.9147 ms** |  **84.1339 ms** | **1.000** |    **0.01** |  **1000.0000** |  **1000.0000** |  **1000.0000** |  **44.09 MB** |        **1.00** |
| Chained      | 20000 | 32    | CPUAc(...) GPU) [26] |  7,288.447 ms |   632.5131 ms |  34.6702 ms | 0.991 |    0.01 |  1000.0000 |  1000.0000 |  1000.0000 |  42.18 MB |        0.96 |
| CpuBaseline  | 20000 | 32    | CPUAc(...) GPU) [26] |      6.309 ms |     4.4251 ms |   0.2426 ms | 0.001 |    0.00 |   390.6250 |   390.6250 |   390.6250 |   5.86 MB |        0.13 |
|              |       |       |                      |               |               |             |       |         |            |            |            |           |             |
| **RoundTripped** | **20000** | **128**   | **CPUAc(...) GPU) [26]** | **29,110.264 ms** | **9,991.8254 ms** | **547.6857 ms** | **1.000** |    **0.02** | **16000.0000** | **15000.0000** | **14000.0000** | **388.65 MB** |        **1.00** |
| Chained      | 20000 | 128   | CPUAc(...) GPU) [26] | 29,249.586 ms | 5,162.4763 ms | 282.9727 ms | 1.005 |    0.02 | 20000.0000 | 19000.0000 | 18000.0000 | 385.11 MB |        0.99 |
| CpuBaseline  | 20000 | 128   | CPUAc(...) GPU) [26] |     15.542 ms |     0.5409 ms |   0.0296 ms | 0.001 |    0.00 |   296.8750 |   296.8750 |   296.8750 |  23.44 MB |        0.06 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Gpu.Benchmarks.TiledSparseDenseProductBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]   : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.13 (10.0.1326.50504), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method      | Documents | Width | Device               | Mean           | Error          | StdDev        | Ratio    | RatioSD | Gen0       | Gen1       | Gen2       | Allocated  | Alloc Ratio |
|------------ |---------- |------ |--------------------- |---------------:|---------------:|--------------:|---------:|--------:|-----------:|-----------:|-----------:|-----------:|------------:|
| **CpuBaseline** | **5000**      | **64**    | **CPUAc(...) GPU) [26]** |       **4.736 ms** |      **0.1452 ms** |     **0.0080 ms** |     **1.00** |    **0.00** |   **148.4375** |   **148.4375** |   **148.4375** |    **2.44 MB** |        **1.00** |
| GpuResident | 5000      | 64    | CPUAc(...) GPU) [26] |   7,936.388 ms | 10,448.8434 ms |   572.7364 ms | 1,675.93 |  104.77 |          - |          - |          - |   20.95 MB |        8.58 |
| GpuFromHost | 5000      | 64    | CPUAc(...) GPU) [26] |   7,583.749 ms |  3,533.5361 ms |   193.6850 ms | 1,601.47 |   35.50 |          - |          - |          - |   23.26 MB |        9.53 |
|             |           |       |                      |                |                |               |          |         |            |            |            |            |             |
| **CpuBaseline** | **5000**      | **256**   | **CPUAc(...) GPU) [26]** |      **15.436 ms** |      **2.7440 ms** |     **0.1504 ms** |     **1.00** |    **0.01** |   **125.0000** |   **125.0000** |   **125.0000** |    **9.77 MB** |        **1.00** |
| GpuResident | 5000      | 256   | CPUAc(...) GPU) [26] |  30,068.332 ms |  6,222.5518 ms |   341.0791 ms | 1,948.06 |   25.18 |  1000.0000 |          - |          - |  139.74 MB |       14.31 |
| GpuFromHost | 5000      | 256   | CPUAc(...) GPU) [26] |  29,886.177 ms |  6,531.8107 ms |   358.0306 ms | 1,936.26 |   25.85 |  1000.0000 |          - |          - |  146.05 MB |       14.96 |
|             |           |       |                      |                |                |               |          |         |            |            |            |            |             |
| **CpuBaseline** | **50000**     | **64**    | **CPUAc(...) GPU) [26]** |      **46.911 ms** |      **1.4874 ms** |     **0.0815 ms** |     **1.00** |    **0.00** |   **181.8182** |   **181.8182** |   **181.8182** |   **24.41 MB** |        **1.00** |
| GpuResident | 50000     | 64    | CPUAc(...) GPU) [26] |  74,761.614 ms | 15,795.4996 ms |   865.8046 ms | 1,593.68 |   16.16 | 10000.0000 |  9000.0000 |  8000.0000 |  519.77 MB |       21.29 |
| GpuFromHost | 50000     | 64    | CPUAc(...) GPU) [26] |  75,025.414 ms | 14,813.2074 ms |   811.9619 ms | 1,599.30 |   15.18 | 11000.0000 | 10000.0000 |  9000.0000 |  546.85 MB |       22.40 |
|             |           |       |                      |                |                |               |          |         |            |            |            |            |             |
| **CpuBaseline** | **50000**     | **256**   | **CPUAc(...) GPU) [26]** |     **168.351 ms** |     **79.0823 ms** |     **4.3348 ms** |     **1.00** |    **0.03** |   **250.0000** |   **250.0000** |   **250.0000** |   **97.66 MB** |        **1.00** |
| GpuResident | 50000     | 256   | CPUAc(...) GPU) [26] | 308,955.099 ms | 21,708.5470 ms | 1,189.9187 ms | 1,836.01 |   42.01 | 83000.0000 | 82000.0000 | 72000.0000 |  7354.9 MB |       75.31 |
| GpuFromHost | 50000     | 256   | CPUAc(...) GPU) [26] | 303,791.917 ms | 16,859.3839 ms |   924.1197 ms | 1,805.33 |   41.14 | 81000.0000 | 80000.0000 | 70000.0000 | 7381.63 MB |       75.59 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `survival-aalen`
- `ols`

### compare-ols

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| ols_summary_n1000 | 0.047 | 3.004 | 64.52x | 0.047 | 3.004 | 64.52x |
| ols_hac_n1000 | 0.157 | 3.470 | 22.09x | 0.157 | 3.470 | 22.09x |
| ols_cluster_n1000 | 0.061 | 3.525 | 58.26x | 0.061 | 3.525 | 58.27x |
| ols_summary_n10000 | 0.434 | 11.447 | 26.35x | 0.434 | 11.446 | 26.35x |
| ols_hac_n10000 | 1.611 | 12.664 | 7.86x | 1.619 | 12.662 | 7.82x |
| ols_cluster_n10000 | 0.590 | 12.959 | 21.97x | 0.594 | 12.958 | 21.82x |
| ols_summary_n100000 | 4.290 | 109.384 | 25.50x | 4.312 | 436.645 | 101.27x |
| ols_hac_n100000 | 16.224 | 119.053 | 7.34x | 16.525 | 475.314 | 28.76x |
| ols_cluster_n100000 | 6.521 | 123.780 | 18.98x | 6.731 | 493.759 | 73.35x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-survival-aalen

```text
Python: {'lifelines': '0.30.3', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| fit_1000 | 8.363 | 87.020 | 10.41x | 8.364 | 347.372 | 41.53x |
| fit_penalised_weighted_1000 | 7.938 | 94.798 | 11.94x | 7.938 | 378.160 | 47.64x |
| predict_survival_1000 | 0.835 | 2.414 | 2.89x | 0.842 | 2.414 | 2.87x |
| fit_10000 | 231.807 | 626.960 | 2.70x | 232.043 | 1002.737 | 4.32x |
| fit_penalised_weighted_10000 | 215.447 | 654.094 | 3.04x | 215.687 | 1027.258 | 4.76x |
| predict_survival_10000 | 3.354 | 5.056 | 1.51x | 3.542 | 20.168 | 5.69x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `AddedTokenScanBenchmarks`
- `BertNormalizerBenchmarks`
- `BkTreeNearestBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
- `ClusteringAgreementBenchmarks`
- `DamerauLevenshteinBenchmarks`
- `DecompositionWidthBenchmarks`
- `DistributionIncumbentBenchmarks`
- `DistributionTailBenchmarks`
- `DoubleMetaphoneBenchmarks`
- `EmbeddingSearchBenchmarks`
- `EmbeddingSearchHelpersBenchmarks`
- `EncoderIncumbentBenchmarks`
- `FilteredVectorSearchBenchmarks`
- `GlmBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `GlmOffsetBenchmarks`
- `GlmPoissonBenchmarks`
- `GlsBenchmarks`
- `HacClusterBenchmarks`
- `HouseholderQrBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `KsExactTableBenchmarks`
- `LeastSquaresRoutingBenchmarks`
- `MannWhitneyExactBenchmarks`
- `MathNetInteropBenchmarks`
- `MultiClassRocAucBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `MultinomialLogitBenchmarks`
- `OlsBenchmarks`
- `OsaBenchmarks`
- `PartitionValidityBenchmarks`
- `PrecompiledNormalizerBenchmarks`
- `ProcessExtractBenchmarks`
- `QgramBenchmarks`
- `QuantileBenchmarks`
- `RankTestBenchmarks`
- `RankingMetricsBenchmarks`
- `RatcliffObershelpBenchmarks`
- `RobustCovarianceBenchmarks`
- `RobustScalerSparseBenchmarks`
- `ScalerIncumbentBenchmarks`
- `SentencePieceBpeLineageBenchmarks`
- `SerialCorrelationBenchmarks`
- `SilhouetteBenchmarks`
- `SplitterIncumbentBenchmarks`
- `StationarityBenchmarks`
- `StatsBenchmarks`
- `TextRankBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TopKAccuracyBenchmarks`
- `TransformerIncumbentBenchmarks`
- `VectorAutoregressionBenchmarks`
- `VectorStoreChurnBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Asked for last

Each was cut at its phase's stop, tonight or on an earlier night and not run since, or left unsettled by a step that died. They are asked for after everything else, so one that does not fit the window takes only what the rest leave.

- `CosineTopKSelectionBenchmarks`
- `TiledCosineTopKBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

No ratio stepped past its noise or drifted over ten days tonight.
