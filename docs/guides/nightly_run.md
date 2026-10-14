# Nightly benchmark run

<!-- nightly-baseline: 7c23e46a8c3f9158c938fc49d51d64513e47d5fa -->
<!-- nightly-owed: AddedTokenScanBenchmarks AgglomerativeIncumbentBenchmarks ArtifactLoadOnlyBenchmarks ArtifactTimeBenchmarks AsyncArtifactBenchmarks BatchEmbeddingBenchmarks BertNormalizerBenchmarks BitParallelEditDistanceBenchmarks BkTreeBenchmarks BkTreeNearestBenchmarks BlockedTableBenchmarks Bm25Benchmarks BpeBenchmarks BpeScalingBenchmarks BpeWordCacheBenchmarks BucketRouteDiagnostics CdistCutoffBenchmarks CdistIncumbentBenchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks ClusteringAgreementBenchmarks CorrelationBenchmarks CosineTopKSelectionBenchmarks DamerauLevenshteinBenchmarks DbscanDimensionBenchmarks DbscanIncumbentBenchmarks DecompositionBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchBenchmarks EmbeddingSearchHelpersBenchmarks EncoderIncumbentBenchmarks FilteredVectorSearchBenchmarks FuzzBenchmarks FuzzCodePointBenchmarks FuzzIncumbentBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks IndelBenchmarks IndelCodePointBenchmarks IndelPatternBenchmarks KMeansFitIncumbentBenchmarks KMeansLloydIncumbentBenchmarks KolmogorovDurbinBenchmarks KsAutoBenchmarks KsExactTableBenchmarks LcsGateBenchmarks LeastSquaresRoutingBenchmarks LevenshteinBenchmarks LevenshteinCodePointBenchmarks LevenshteinIncumbentBenchmarks MannWhitneyExactBenchmarks ManyLabelsMetricsBenchmarks MathNetInteropBenchmarks MetaNumericsPcaBenchmarks MetaNumericsStatsBenchmarks MetricsBenchmarks MetricsIncumbentBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks MyersGateBenchmarks OlsBenchmarks OsaBenchmarks PartialFitBenchmarks PartialRatioLongNeedleBenchmarks PartitionValidityBenchmarks PersistenceBenchmarks PrecompiledNormalizerBenchmarks PrincipalComponentVarianceBenchmarks ProcessExtractBenchmarks QgramBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RatcliffObershelpBenchmarks RegressionMetricsBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks ScalerIncumbentBenchmarks SentencePieceBpeLineageBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks SimilaritySketchBenchmarks SmallArtifactBenchmarks SplitterIncumbentBenchmarks StationarityBenchmarks StatsBenchmarks StopWordBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks TokenPatternBenchmarks TokenizerIncumbentBenchmarks TopKAccuracyBenchmarks TransformerIncumbentBenchmarks VarianceAndFitBenchmarks VectorAutoregressionBenchmarks VectorMathBenchmarks VectorStoreChurnBenchmarks VectorizerBenchmarks VectorizerIncumbentBenchmarks WeightedLeastSquaresBenchmarks cdist cluster-weighted cox-extended cross-conformal glm iv nmf-transform ols onehot panel survival-aalen survival-family survival-parametric transformers var -->
<!-- nightly-cut: TiledCosineTopKBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance.md). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest.md).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `7c23e46a8c3f9158c938fc49d51d64513e47d5fa`
- Previous run: `7c23e46a8c3f9158c938fc49d51d64513e47d5fa`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run:

- `CoxBenchmarks`
- `SurvivalBenchmarks`

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

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

| Method | SampleSize | Covariates | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **463.0 μs** |    **16.10 μs** |   **0.88 μs** |   **8.3008** |   **1.4648** |        **-** |  **139.09 KB** |
| **Fit**    | **1000**       | **8**          |  **1,916.1 μs** |    **42.07 μs** |   **2.31 μs** |  **11.7188** |   **1.9531** |        **-** |  **196.98 KB** |
| **Fit**    | **10000**      | **2**          |  **5,428.9 μs** |   **512.37 μs** |  **28.08 μs** | **164.0625** | **164.0625** | **164.0625** | **1247.39 KB** |
| **Fit**    | **10000**      | **8**          | **20,448.9 μs** | **2,543.12 μs** | **139.40 μs** | **281.2500** | **250.0000** | **250.0000** |  **1734.9 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

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

| Method              | SampleSize | DistinctPercent | Mean         | Error        | StdDev    | Ratio | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |-------------:|-------------:|----------:|------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |     **16.42 μs** |     **0.982 μs** |  **0.054 μs** |  **1.00** |    **0.9766** |         **-** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |     15.23 μs |     4.568 μs |  0.250 μs |  0.93 |    0.9308 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     21.75 μs |     1.574 μs |  0.086 μs |  1.32 |    1.6174 |    0.0610 |        - |   26.84 KB |        1.68 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **59.59 μs** |     **6.032 μs** |  **0.331 μs** |  **1.00** |    **4.3945** |    **0.4883** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     23.42 μs |     8.178 μs |  0.448 μs |  0.39 |    3.7842 |    0.3967 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     48.66 μs |     5.018 μs |  0.275 μs |  0.82 |    2.9907 |    0.3052 |        - |   49.57 KB |        0.69 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **8,704.26 μs** |   **360.805 μs** | **19.777 μs** |  **1.00** |  **328.1250** |  **312.5000** | **312.5000** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  8,536.26 μs | 1,246.004 μs | 68.298 μs |  0.98 |  328.1250 |  312.5000 | 312.5000 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               | 10,004.64 μs |   981.577 μs | 53.804 μs |  1.15 |  484.3750 |  484.3750 | 484.3750 | 2598.82 KB |        1.71 |
|                     |            |                 |              |              |           |       |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **15,862.51 μs** |   **969.664 μs** | **53.151 μs** |  **1.00** | **1125.0000** | **1093.7500** | **968.7500** | **6650.02 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             | 13,039.73 μs | 1,148.164 μs | 62.935 μs |  0.82 |  843.7500 |  828.1250 | 687.5000 | 5660.23 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 16,195.42 μs |   829.808 μs | 45.485 μs |  1.02 |  500.0000 |  500.0000 | 500.0000 | 4916.39 KB |        0.74 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `levenshtein`
- `indel`
- `metrics`
- `persistence`
- `stats`
- `splitters`

### compare-indel

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.13 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 118.8 | 27.9 | 4.25x C# faster |
| latin | 32 | 181.5 | 88.2 | 2.06x C# faster |
| latin | 128 | 481.7 | 425.2 | 1.13x C# faster |
| latin | 512 | 4494.1 | 4987.9 | 1.11x Py faster |
| cjk | 8 | 144.4 | 26.1 | 5.53x C# faster |
| cjk | 32 | 333.2 | 220.4 | 1.51x C# faster |
| cjk | 128 | 1913.8 | 1379.2 | 1.39x C# faster |
| cjk | 512 | 14860.6 | 9003.1 | 1.65x C# faster |

Note: Indel is len(a)+len(b)-2*LCS on both sides, so this compares the subsequence kernels. Lodestar's is Hyyro's bit-parallel LLCS above a pattern of 8 and a rolling-row dynamic program below it (#273).

<!-- markdownlint-enable MD060 -->

### compare-levenshtein

```text
Python: rapidfuzz 3.14.6 (py 3.12.15)
C#:     Lodestar.Text on .NET 10.0.13 (mode Utf16Unit)
```

<!-- markdownlint-disable MD060 -->

| alphabet | length | Python ns/pair | C# ns/pair | speedup (py/C#) |
|---|---:|---:|---:|:---|
| latin | 8 | 152.4 | 18.5 | 8.24x C# faster |
| latin | 32 | 284.3 | 158.0 | 1.80x C# faster |
| latin | 128 | 1694.3 | 887.6 | 1.91x C# faster |
| latin | 512 | 14191.7 | 11541.8 | 1.23x C# faster |
| cjk | 8 | 161.9 | 17.9 | 9.06x C# faster |
| cjk | 32 | 386.9 | 289.6 | 1.34x C# faster |
| cjk | 128 | 2977.3 | 2015.1 | 1.48x C# faster |
| cjk | 512 | 23604.6 | 17791.9 | 1.33x C# faster |

Note: Python times the realistic per-call loop; rapidfuzz's C core uses the bit-parallel Myers algorithm, so it scales better on long strings.

<!-- markdownlint-enable MD060 -->

### compare-metrics

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| confusion_matrix_n1000_k2 | 0.004 | 0.988 | 226.12x | 0.004 | 0.988 | 226.09x |
| accuracy_n1000_k2 | 0.000 | 0.527 | 4352.58x | 0.000 | 0.527 | 4352.18x |
| precision_recall_f1_macro_n1000_k2 | 0.005 | 1.819 | 354.38x | 0.005 | 1.819 | 354.35x |
| classification_report_n1000_k2 | 0.007 | 7.018 | 954.51x | 0.007 | 7.017 | 954.50x |
| roc_auc_binary_n1000_k2 | 0.018 | 1.961 | 109.07x | 0.018 | 1.961 | 108.99x |
| balanced_accuracy_n1000_k2 | 0.004 | 1.047 | 235.42x | 0.004 | 1.047 | 235.45x |
| matthews_n1000_k2 | 0.005 | 2.041 | 439.24x | 0.005 | 2.041 | 439.30x |
| cohen_kappa_n1000_k2 | 0.004 | 1.099 | 245.92x | 0.004 | 1.099 | 245.91x |
| mse_n1000_k2 | 0.000 | 0.313 | 647.28x | 0.000 | 0.313 | 647.22x |
| mae_n1000_k2 | 0.000 | 0.312 | 642.89x | 0.000 | 0.312 | 642.69x |
| median_ae_n1000_k2 | 0.006 | 0.322 | 55.61x | 0.006 | 0.322 | 55.61x |
| r2_n1000_k2 | 0.001 | 0.379 | 318.96x | 0.001 | 0.379 | 318.96x |
| confusion_matrix_n1000_k10 | 0.005 | 0.991 | 190.52x | 0.005 | 0.991 | 190.52x |
| accuracy_n1000_k10 | 0.000 | 0.534 | 4414.59x | 0.000 | 0.534 | 4415.18x |
| precision_recall_f1_macro_n1000_k10 | 0.006 | 1.831 | 310.99x | 0.006 | 1.831 | 310.98x |
| classification_report_n1000_k10 | 0.012 | 7.072 | 595.77x | 0.012 | 7.072 | 595.77x |
| roc_auc_ovr_macro_n1000_k10 | 0.524 | 9.865 | 18.83x | 0.524 | 9.863 | 18.82x |
| balanced_accuracy_n1000_k10 | 0.006 | 1.053 | 183.70x | 0.006 | 1.053 | 183.68x |
| matthews_n1000_k10 | 0.006 | 2.048 | 362.52x | 0.006 | 2.048 | 362.55x |
| cohen_kappa_n1000_k10 | 0.006 | 1.097 | 180.89x | 0.006 | 1.096 | 180.85x |
| mse_n1000_k10 | 0.000 | 0.309 | 634.08x | 0.000 | 0.309 | 634.07x |
| mae_n1000_k10 | 0.000 | 0.309 | 633.10x | 0.000 | 0.309 | 633.09x |
| median_ae_n1000_k10 | 0.006 | 0.319 | 56.22x | 0.006 | 0.319 | 56.21x |
| r2_n1000_k10 | 0.001 | 0.378 | 317.01x | 0.001 | 0.378 | 317.03x |
| confusion_matrix_n100000_k2 | 0.460 | 10.919 | 23.71x | 0.460 | 10.917 | 23.71x |
| accuracy_n100000_k2 | 0.012 | 3.804 | 313.26x | 0.012 | 3.803 | 313.40x |
| precision_recall_f1_macro_n100000_k2 | 0.460 | 12.543 | 27.25x | 0.460 | 12.541 | 27.25x |
| classification_report_n100000_k2 | 0.464 | 27.655 | 59.66x | 0.463 | 27.651 | 59.66x |
| roc_auc_binary_n100000_k2 | 4.264 | 27.665 | 6.49x | 4.264 | 27.661 | 6.49x |
| balanced_accuracy_n100000_k2 | 0.457 | 10.924 | 23.89x | 0.457 | 10.922 | 23.89x |
| matthews_n100000_k2 | 0.449 | 22.062 | 49.09x | 0.450 | 22.062 | 49.01x |
| cohen_kappa_n100000_k2 | 0.453 | 11.058 | 24.39x | 0.453 | 11.056 | 24.39x |
| mse_n100000_k2 | 0.045 | 0.469 | 10.37x | 0.045 | 0.469 | 10.37x |
| mae_n100000_k2 | 0.045 | 0.458 | 10.20x | 0.045 | 0.458 | 10.20x |
| median_ae_n100000_k2 | 0.685 | 1.822 | 2.66x | 0.704 | 1.822 | 2.59x |
| r2_n100000_k2 | 0.110 | 0.706 | 6.44x | 0.110 | 0.706 | 6.44x |
| confusion_matrix_n100000_k10 | 0.492 | 11.041 | 22.44x | 0.492 | 11.036 | 22.43x |
| accuracy_n100000_k10 | 0.011 | 3.836 | 344.51x | 0.011 | 3.835 | 344.49x |
| precision_recall_f1_macro_n100000_k10 | 0.493 | 13.141 | 26.65x | 0.493 | 13.139 | 26.65x |
| classification_report_n100000_k10 | 0.500 | 30.233 | 60.47x | 0.500 | 30.229 | 60.47x |
| roc_auc_ovr_macro_n100000_k10 | 33.352 | 224.751 | 6.74x | 33.350 | 224.726 | 6.74x |
| balanced_accuracy_n100000_k10 | 0.493 | 11.106 | 22.52x | 0.493 | 11.104 | 22.52x |
| matthews_n100000_k10 | 0.493 | 22.808 | 46.26x | 0.493 | 22.806 | 46.26x |
| cohen_kappa_n100000_k10 | 0.493 | 11.208 | 22.71x | 0.493 | 11.206 | 22.71x |
| mse_n100000_k10 | 0.045 | 0.463 | 10.24x | 0.045 | 0.463 | 10.23x |
| mae_n100000_k10 | 0.045 | 0.456 | 10.13x | 0.045 | 0.455 | 10.13x |
| median_ae_n100000_k10 | 0.686 | 1.804 | 2.63x | 0.734 | 1.804 | 2.46x |
| r2_n100000_k10 | 0.110 | 0.709 | 6.46x | 0.110 | 0.709 | 6.46x |
| confusion_matrix_n1000000_k2 | 4.605 | 102.232 | 22.20x | 4.605 | 102.196 | 22.19x |
| accuracy_n1000000_k2 | 0.135 | 33.913 | 251.69x | 0.135 | 33.910 | 251.68x |
| precision_recall_f1_macro_n1000000_k2 | 4.602 | 109.377 | 23.77x | 4.602 | 109.345 | 23.76x |
| classification_report_n1000000_k2 | 4.609 | 214.255 | 46.48x | 4.609 | 214.219 | 46.48x |
| roc_auc_binary_n1000000_k2 | 56.208 | 296.551 | 5.28x | 56.202 | 296.506 | 5.28x |
| balanced_accuracy_n1000000_k2 | 4.602 | 101.397 | 22.03x | 4.602 | 101.385 | 22.03x |
| matthews_n1000000_k2 | 4.602 | 203.869 | 44.30x | 4.602 | 203.843 | 44.30x |
| cohen_kappa_n1000000_k2 | 4.602 | 101.901 | 22.14x | 4.602 | 101.888 | 22.14x |
| mse_n1000000_k2 | 0.458 | 2.661 | 5.81x | 0.458 | 2.660 | 5.81x |
| mae_n1000000_k2 | 0.452 | 2.495 | 5.52x | 0.452 | 2.495 | 5.52x |
| median_ae_n1000000_k2 | 6.307 | 14.643 | 2.32x | 6.385 | 14.641 | 2.29x |
| r2_n1000000_k2 | 1.100 | 4.678 | 4.25x | 1.100 | 4.678 | 4.25x |
| confusion_matrix_n1000000_k10 | 4.922 | 101.054 | 20.53x | 4.921 | 101.016 | 20.53x |
| accuracy_n1000000_k10 | 0.112 | 33.425 | 297.57x | 0.112 | 33.419 | 297.55x |
| precision_recall_f1_macro_n1000000_k10 | 4.919 | 114.713 | 23.32x | 4.918 | 114.706 | 23.32x |
| classification_report_n1000000_k10 | 4.927 | 237.304 | 48.16x | 4.927 | 237.264 | 48.16x |
| balanced_accuracy_n1000000_k10 | 4.921 | 100.707 | 20.47x | 4.920 | 100.698 | 20.47x |
| matthews_n1000000_k10 | 4.924 | 209.816 | 42.61x | 4.924 | 209.784 | 42.61x |
| cohen_kappa_n1000000_k10 | 4.925 | 100.903 | 20.49x | 4.924 | 100.884 | 20.49x |
| mse_n1000000_k10 | 0.456 | 2.487 | 5.46x | 0.456 | 2.486 | 5.45x |
| mae_n1000000_k10 | 0.452 | 2.488 | 5.50x | 0.452 | 2.488 | 5.50x |
| median_ae_n1000000_k10 | 6.858 | 14.616 | 2.13x | 7.027 | 14.614 | 2.08x |
| r2_n1000000_k10 | 1.119 | 4.674 | 4.18x | 1.119 | 4.673 | 4.18x |

ratio > 1 means Lodestar is faster. cpu is the merge gate for this branch
(docs/guides/performance.md): every operation, every size, must be >= 1x.

<!-- markdownlint-enable MD060 -->

### compare-persistence

```text
Python: {'tokenizers': '0.23.2', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.334 | 9.550 | 1.79x | 5.705 | 9.549 | 1.67x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.169 | 16.680 | 1.37x | 12.490 | 16.677 | 1.34x | 706,526 | 706,526 |
| tokenizer_json_unigram | 13.928 | 37.182 | 2.67x | 14.315 | 37.175 | 2.60x | 1,990,038 | 1,990,038 |
| spiece_model | 7.838 | 28.988 | 3.70x | 8.514 | 28.986 | 3.40x | 533,084 | 533,084 |
| tfidf_save | 1.547 | 2.358 | 1.52x | 1.589 | 2.358 | 1.48x | 581,787 | 591,922 |
| tfidf_load | 4.661 | 4.105 | 0.88x | 4.853 | 4.105 | 0.85x | 581,787 | 591,922 |
| embedding_index_save | 4.416 | 1.546 | 0.35x | 4.592 | 1.546 | 0.34x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 49.181 | 37.365 | 0.76x | 10.668 | 5.147 | 0.48x | 20,589,007 | 15,360,128 |
| embedding_index_load | 5.699 | 1.469 | 0.26x | 6.042 | 1.469 | 0.24x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 7.531 | 1.009 | 0.13x | 7.861 | 1.006 | 0.13x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 4.607 | 1.486 | 0.32x | 4.929 | 1.486 | 0.30x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.370 | 1.513 | 1.10x | 1.520 | 1.513 | 1.00x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 92.16x | 0.000 | 0.001 | 92.17x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 409.006 | 554.567 | 1.36x | 408.983 | 554.543 | 1.36x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 71.870 | 66.776 | 0.93x | 72.253 | 66.771 | 0.92x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->

### compare-splitters

```text
Python: {'scikit-learn': '1.9.1', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| kfold_n10000 | 0.086 | 0.116 | 1.36x | 0.086 | 0.116 | 1.36x |
| stratified_n10000 | 0.178 | 0.870 | 4.88x | 0.178 | 0.870 | 4.88x |
| traintest_n10000 | 0.005 | 0.234 | 50.89x | 0.005 | 0.234 | 50.89x |
| kfold_seeded_n10000 | 0.306 | 0.673 | 2.20x | 0.306 | 0.673 | 2.20x |
| stratified_seeded_n10000 | 0.421 | 1.479 | 3.51x | 0.421 | 1.479 | 3.51x |
| traintest_seeded_n10000 | 0.160 | 0.523 | 3.27x | 0.160 | 0.523 | 3.27x |
| stratified_traintest_n10000 | 0.665 | 2.761 | 4.15x | 0.665 | 2.761 | 4.15x |
| group_kfold_n10000 | 0.123 | 0.600 | 4.89x | 0.123 | 0.600 | 4.89x |
| stratified_group_kfold_n10000 | 0.240 | 32.162 | 133.75x | 0.240 | 32.158 | 133.74x |
| timeseries_n10000 | 0.000 | 0.012 | 97.09x | 0.000 | 0.012 | 97.09x |
| repeated_kfold_n10000 | 0.959 | 1.790 | 1.87x | 0.959 | 1.790 | 1.87x |
| kfold_n100000 | 1.154 | 2.537 | 2.20x | 1.322 | 2.537 | 1.92x |
| stratified_n100000 | 2.154 | 7.131 | 3.31x | 2.297 | 7.129 | 3.10x |
| traintest_n100000 | 0.213 | 0.405 | 1.90x | 0.215 | 0.405 | 1.88x |
| kfold_seeded_n100000 | 3.574 | 6.876 | 1.92x | 3.720 | 6.876 | 1.85x |
| stratified_seeded_n100000 | 4.736 | 11.383 | 2.40x | 4.934 | 11.382 | 2.31x |
| traintest_seeded_n100000 | 2.047 | 2.001 | 0.98x | 2.089 | 2.001 | 0.96x |
| stratified_traintest_n100000 | 8.099 | 23.574 | 2.91x | 8.197 | 23.572 | 2.88x |
| group_kfold_n100000 | 1.579 | 6.634 | 4.20x | 1.719 | 6.634 | 3.86x |
| stratified_group_kfold_n100000 | 2.740 | 326.937 | 119.32x | 2.890 | 326.897 | 113.10x |
| timeseries_n100000 | 0.000 | 0.042 | 334.93x | 0.000 | 0.042 | 334.92x |
| repeated_kfold_n100000 | 10.077 | 19.007 | 1.89x | 10.266 | 19.004 | 1.85x |
| kfold_n1000000 | 9.475 | 17.625 | 1.86x | 10.117 | 17.619 | 1.74x |
| stratified_n1000000 | 20.117 | 66.740 | 3.32x | 20.764 | 66.730 | 3.21x |
| traintest_n1000000 | 0.481 | 2.075 | 4.32x | 0.553 | 2.075 | 3.76x |
| kfold_seeded_n1000000 | 36.678 | 58.600 | 1.60x | 37.190 | 58.597 | 1.58x |
| stratified_seeded_n1000000 | 50.389 | 106.692 | 2.12x | 51.870 | 106.687 | 2.06x |
| traintest_seeded_n1000000 | 20.836 | 19.222 | 0.92x | 21.213 | 19.220 | 0.91x |
| stratified_traintest_n1000000 | 93.527 | 238.394 | 2.55x | 94.047 | 238.373 | 2.53x |
| group_kfold_n1000000 | 14.488 | 52.317 | 3.61x | 15.084 | 52.315 | 3.47x |
| stratified_group_kfold_n1000000 | 26.823 | 3247.987 | 121.09x | 27.573 | 3247.655 | 117.78x |
| timeseries_n1000000 | 0.000 | 0.328 | 3154.31x | 0.000 | 0.328 | 3154.15x |
| repeated_kfold_n1000000 | 106.660 | 177.725 | 1.67x | 108.281 | 177.718 | 1.64x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

### compare-stats

```text
Python: {'scipy': '1.18.1', 'statsmodels': '0.15.0', 'numpy': '2.5.3'} (py 3.12.15)
C#:     Lodestar on .NET 10.0.13
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu |
|:---|---:|---:|---:|---:|---:|---:|
| welch_t_n1000 | 0.006 | 0.815 | 147.76x | 0.006 | 0.815 | 147.74x |
| mann_whitney_n1000 | 0.029 | 0.743 | 25.61x | 0.029 | 0.743 | 25.60x |
| chi_square_n1000 | 0.000 | 0.345 | 1522.27x | 0.000 | 0.345 | 1522.27x |
| fligner_n1000 | 0.078 | 0.820 | 10.56x | 0.078 | 0.820 | 10.56x |
| anderson_ksamp_n1000 | 0.165 | 0.399 | 2.42x | 0.165 | 0.399 | 2.42x |
| spearman_matrix_n1000 | 0.114 | 0.931 | 8.19x | 0.114 | 0.931 | 8.19x |
| pointbiserial_n1000 | 0.011 | 0.579 | 53.02x | 0.011 | 0.579 | 53.02x |
| welch_t_n10000 | 0.052 | 0.867 | 16.57x | 0.052 | 0.867 | 16.57x |
| mann_whitney_n10000 | 0.766 | 2.853 | 3.72x | 0.766 | 2.852 | 3.72x |
| chi_square_n10000 | 0.001 | 0.349 | 257.23x | 0.001 | 0.349 | 257.22x |
| fligner_n10000 | 1.424 | 3.262 | 2.29x | 1.494 | 3.261 | 2.18x |
| anderson_ksamp_n10000 | 2.057 | 4.002 | 1.95x | 2.185 | 4.002 | 1.83x |
| spearman_matrix_n10000 | 2.355 | 4.838 | 2.05x | 2.415 | 4.838 | 2.00x |
| pointbiserial_n10000 | 0.103 | 0.646 | 6.30x | 0.103 | 0.645 | 6.30x |
| welch_t_n100000 | 0.519 | 1.269 | 2.45x | 0.518 | 1.269 | 2.45x |
| mann_whitney_n100000 | 8.130 | 28.815 | 3.54x | 8.128 | 28.813 | 3.54x |
| chi_square_n100000 | 0.014 | 0.369 | 25.44x | 0.014 | 0.369 | 25.43x |
| fligner_n100000 | 13.576 | 32.767 | 2.41x | 13.840 | 32.762 | 2.37x |
| anderson_ksamp_n100000 | 15.154 | 44.050 | 2.91x | 15.529 | 44.045 | 2.84x |
| spearman_matrix_n100000 | 19.047 | 52.191 | 2.74x | 19.464 | 52.190 | 2.68x |
| pointbiserial_n100000 | 1.326 | 1.432 | 1.08x | 1.343 | 5.714 | 4.26x |

ratio > 1 means Lodestar is faster. cpu is the honest one on both sides:
elapsed time hides .NET's background GC threads, and numpy's BLAS
threads too -- statsmodels at 100 000 rows spends ~10x more cpu than wall.

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `AddedTokenScanBenchmarks`
- `AgglomerativeIncumbentBenchmarks`
- `ArtifactLoadOnlyBenchmarks`
- `ArtifactTimeBenchmarks`
- `AsyncArtifactBenchmarks`
- `BatchEmbeddingBenchmarks`
- `BertNormalizerBenchmarks`
- `BitParallelEditDistanceBenchmarks`
- `BkTreeBenchmarks`
- `BkTreeNearestBenchmarks`
- `BlockedTableBenchmarks`
- `Bm25Benchmarks`
- `BpeBenchmarks`
- `BpeScalingBenchmarks`
- `BpeWordCacheBenchmarks`
- `BucketRouteDiagnostics`
- `CdistCutoffBenchmarks`
- `CdistIncumbentBenchmarks`
- `ChainedProductBenchmarks`
- `ClassificationReportBenchmarks`
- `ClassifierCurveBenchmarks`
- `ClusteringAgreementBenchmarks`
- `CorrelationBenchmarks`
- `CosineTopKSelectionBenchmarks`
- `DamerauLevenshteinBenchmarks`
- `DbscanDimensionBenchmarks`
- `DbscanIncumbentBenchmarks`
- `DecompositionBenchmarks`
- `DecompositionWidthBenchmarks`
- `DistributionIncumbentBenchmarks`
- `DistributionTailBenchmarks`
- `DoubleMetaphoneBenchmarks`
- `EmbeddingSearchBenchmarks`
- `EmbeddingSearchHelpersBenchmarks`
- `EncoderIncumbentBenchmarks`
- `FilteredVectorSearchBenchmarks`
- `FuzzBenchmarks`
- `FuzzCodePointBenchmarks`
- `FuzzIncumbentBenchmarks`
- `GlmBenchmarks`
- `GlmNegativeBinomialBenchmarks`
- `GlmOffsetBenchmarks`
- `GlmPoissonBenchmarks`
- `GlsBenchmarks`
- `HacClusterBenchmarks`
- `HouseholderQrBenchmarks`
- `IndelBenchmarks`
- `IndelCodePointBenchmarks`
- `IndelPatternBenchmarks`
- `KMeansFitIncumbentBenchmarks`
- `KMeansLloydIncumbentBenchmarks`
- `KolmogorovDurbinBenchmarks`
- `KsAutoBenchmarks`
- `KsExactTableBenchmarks`
- `LcsGateBenchmarks`
- `LeastSquaresRoutingBenchmarks`
- `LevenshteinBenchmarks`
- `LevenshteinCodePointBenchmarks`
- `LevenshteinIncumbentBenchmarks`
- `MannWhitneyExactBenchmarks`
- `ManyLabelsMetricsBenchmarks`
- `MathNetInteropBenchmarks`
- `MetaNumericsPcaBenchmarks`
- `MetaNumericsStatsBenchmarks`
- `MetricsBenchmarks`
- `MetricsIncumbentBenchmarks`
- `MultiClassRocAucBenchmarks`
- `MultilabelConfusionMatrixBenchmarks`
- `MultinomialLogitBenchmarks`
- `MyersGateBenchmarks`
- `OlsBenchmarks`
- `OsaBenchmarks`
- `PartialFitBenchmarks`
- `PartialRatioLongNeedleBenchmarks`
- `PartitionValidityBenchmarks`
- `PersistenceBenchmarks`
- `PrecompiledNormalizerBenchmarks`
- `PrincipalComponentVarianceBenchmarks`
- `ProcessExtractBenchmarks`
- `QgramBenchmarks`
- `QuantileBenchmarks`
- `RankTestBenchmarks`
- `RankingMetricsBenchmarks`
- `RatcliffObershelpBenchmarks`
- `RegressionMetricsBenchmarks`
- `RobustCovarianceBenchmarks`
- `RobustScalerSparseBenchmarks`
- `ScalerIncumbentBenchmarks`
- `SentencePieceBpeLineageBenchmarks`
- `SerialCorrelationBenchmarks`
- `SilhouetteBenchmarks`
- `SimilaritySketchBenchmarks`
- `SmallArtifactBenchmarks`
- `SplitterIncumbentBenchmarks`
- `StationarityBenchmarks`
- `StatsBenchmarks`
- `StopWordBenchmarks`
- `TextRankBenchmarks`
- `TiledMinHashSignaturesBenchmarks`
- `TiledSparseDenseProductBenchmarks`
- `TokenPatternBenchmarks`
- `TokenizerIncumbentBenchmarks`
- `TopKAccuracyBenchmarks`
- `TransformerIncumbentBenchmarks`
- `VarianceAndFitBenchmarks`
- `VectorAutoregressionBenchmarks`
- `VectorMathBenchmarks`
- `VectorStoreChurnBenchmarks`
- `VectorizerBenchmarks`
- `VectorizerIncumbentBenchmarks`
- `WeightedLeastSquaresBenchmarks`

## Comparisons selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `cdist`
- `cluster-weighted`
- `cox-extended`
- `cross-conformal`
- `glm`
- `iv`
- `nmf-transform`
- `ols`
- `onehot`
- `panel`
- `survival-aalen`
- `survival-family`
- `survival-parametric`
- `transformers`
- `var`

## Cut at the stop

These were still running when their phase's time ran out and were killed with their process group. They are asked for first tomorrow; cut again, they are failed as too long for the window.

- `TiledCosineTopKBenchmarks`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

No ratio stepped past its noise or drifted over ten days tonight.
