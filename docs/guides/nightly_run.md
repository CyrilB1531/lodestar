# Nightly benchmark run

<!-- nightly-baseline: 59a57f7257ac13a8a57dcee55e218a7f45b3b458 -->
<!-- nightly-owed: AddedTokenScanBenchmarks BertNormalizerBenchmarks BitParallelEditDistanceBenchmarks BkTreeNearestBenchmarks ChainedProductBenchmarks ClassificationReportBenchmarks ClassifierCurveBenchmarks ClusteringAgreementBenchmarks CosineTopKSelectionBenchmarks DamerauLevenshteinBenchmarks DecompositionWidthBenchmarks DistributionIncumbentBenchmarks DistributionTailBenchmarks DoubleMetaphoneBenchmarks EmbeddingSearchBenchmarks EmbeddingSearchHelpersBenchmarks EncoderIncumbentBenchmarks FilteredVectorSearchBenchmarks GlmBenchmarks GlmNegativeBinomialBenchmarks GlmOffsetBenchmarks GlmPoissonBenchmarks GlsBenchmarks HacClusterBenchmarks HouseholderQrBenchmarks KolmogorovDurbinBenchmarks KsExactTableBenchmarks LeastSquaresRoutingBenchmarks MannWhitneyExactBenchmarks MathNetInteropBenchmarks MultiClassRocAucBenchmarks MultilabelConfusionMatrixBenchmarks MultinomialLogitBenchmarks OlsBenchmarks OsaBenchmarks PartitionValidityBenchmarks PrecompiledNormalizerBenchmarks ProcessExtractBenchmarks QgramBenchmarks QuantileBenchmarks RankTestBenchmarks RankingMetricsBenchmarks RatcliffObershelpBenchmarks RobustCovarianceBenchmarks RobustScalerSparseBenchmarks ScalerIncumbentBenchmarks SentencePieceBpeLineageBenchmarks SerialCorrelationBenchmarks SilhouetteBenchmarks SplitterIncumbentBenchmarks StationarityBenchmarks StatsBenchmarks TextRankBenchmarks TiledCosineTopKBenchmarks TiledMinHashSignaturesBenchmarks TiledSparseDenseProductBenchmarks TopKAccuracyBenchmarks TransformerIncumbentBenchmarks VectorAutoregressionBenchmarks VectorStoreChurnBenchmarks WeightedLeastSquaresBenchmarks -->
<!-- nightly-cut: CosineTopKSelectionBenchmarks TiledCosineTopKBenchmarks TiledSparseDenseProductBenchmarks -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance.md). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest.md).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `59a57f7257ac13a8a57dcee55e218a7f45b3b458`
- Previous run: `59a57f7257ac13a8a57dcee55e218a7f45b3b458`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run, and what earlier nights owed:

- `CoxBenchmarks`
- `SurvivalBenchmarks`

### Lodestar.Survival.Benchmarks.CoxBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method | SampleSize | Covariates | Mean        | Error       | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------- |----------- |----------- |------------:|------------:|----------:|---------:|---------:|---------:|-----------:|
| **Fit**    | **1000**       | **2**          |    **331.1 μs** |    **20.53 μs** |   **1.13 μs** |   **8.3008** |   **0.9766** |        **-** |  **140.32 KB** |
| **Fit**    | **1000**       | **8**          |  **2,041.3 μs** |   **205.33 μs** |  **11.25 μs** |  **11.7188** |        **-** |        **-** |  **201.32 KB** |
| **Fit**    | **10000**      | **2**          |  **5,567.9 μs** |   **332.36 μs** |  **18.22 μs** | **164.0625** | **164.0625** | **164.0625** | **1248.62 KB** |
| **Fit**    | **10000**      | **8**          | **18,695.6 μs** | **6,907.10 μs** | **378.60 μs** | **281.2500** | **250.0000** | **250.0000** | **1737.31 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Survival.Benchmarks.SurvivalBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]   : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.15 (10.0.1526.53008), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method              | SampleSize | DistinctPercent | Mean         | Error        | StdDev     | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated  | Alloc Ratio |
|-------------------- |----------- |---------------- |-------------:|-------------:|-----------:|------:|--------:|----------:|----------:|---------:|-----------:|------------:|
| **KaplanMeierEstimate** | **1000**       | **4**               |     **16.27 μs** |     **2.767 μs** |   **0.152 μs** |  **1.00** |    **0.01** |    **0.9766** |         **-** |        **-** |   **15.98 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 4               |     15.02 μs |     0.702 μs |   0.038 μs |  0.92 |    0.01 |    0.9308 |    0.0153 |        - |   15.27 KB |        0.96 |
| LogRankTest         | 1000       | 4               |     21.66 μs |     3.509 μs |   0.192 μs |  1.33 |    0.01 |    1.6174 |    0.0610 |        - |   26.84 KB |        1.68 |
|                     |            |                 |              |              |            |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **1000**       | **100**             |     **59.88 μs** |     **8.940 μs** |   **0.490 μs** |  **1.00** |    **0.01** |    **4.3945** |    **0.4883** |        **-** |      **72 KB** |        **1.00** |
| NelsonAalenEstimate | 1000       | 100             |     22.88 μs |     1.454 μs |   0.080 μs |  0.38 |    0.00 |    3.7842 |    0.3967 |        - |    62.2 KB |        0.86 |
| LogRankTest         | 1000       | 100             |     45.50 μs |     0.990 μs |   0.054 μs |  0.76 |    0.01 |    2.9907 |    0.3052 |        - |   49.57 KB |        0.69 |
|                     |            |                 |              |              |            |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **4**               |  **9,941.86 μs** |   **160.039 μs** |   **8.772 μs** |  **1.00** |    **0.00** |  **328.1250** |  **312.5000** | **312.5000** | **1517.93 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 4               |  9,647.80 μs | 1,281.320 μs |  70.233 μs |  0.97 |    0.01 |  328.1250 |  312.5000 | 312.5000 | 1455.34 KB |        0.96 |
| LogRankTest         | 100000     | 4               | 11,016.75 μs |   694.078 μs |  38.045 μs |  1.11 |    0.00 |  484.3750 |  484.3750 | 484.3750 | 2598.82 KB |        1.71 |
|                     |            |                 |              |              |            |       |         |           |           |          |            |             |
| **KaplanMeierEstimate** | **100000**     | **100**             | **16,903.37 μs** |   **297.976 μs** |  **16.333 μs** |  **1.00** |    **0.00** | **1125.0000** | **1093.7500** | **968.7500** | **6650.01 KB** |        **1.00** |
| NelsonAalenEstimate | 100000     | 100             | 13,593.09 μs | 3,595.161 μs | 197.063 μs |  0.80 |    0.01 |  875.0000 |  828.1250 | 718.7500 | 5660.24 KB |        0.85 |
| LogRankTest         | 100000     | 100             | 17,858.19 μs | 6,967.616 μs | 381.919 μs |  1.06 |    0.02 |  500.0000 |  500.0000 | 500.0000 | 4916.39 KB |        0.74 |

<!-- markdownlint-enable MD060 -->

## Classes selected, and not reached tonight

The run stops starting them when the time left cannot hold one, so these were carried to the next run rather than measured badly or killed mid-flight. They are asked for first tomorrow whether or not anything else changes.

- `AddedTokenScanBenchmarks`
- `BertNormalizerBenchmarks`
- `BitParallelEditDistanceBenchmarks`
- `BkTreeNearestBenchmarks`
- `ChainedProductBenchmarks`
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
- `TiledSparseDenseProductBenchmarks`

## Failed tonight

These exited with an error, raised, shared a script that failed tonight, or are named by the map and declared by no project; the log's annotation on each says which. The run went on, so the rest are on this page. A failure is not owed: it is selected again only when something it reads changes.

- `Bm25Benchmarks`
- `SimilaritySketchBenchmarks`
- `StopWordBenchmarks`
- `TokenPatternBenchmarks`
- `VectorMathBenchmarks`
- `VectorizerBenchmarks`
- `cluster-weighted`
- `cox-extended`
- `cross-conformal`
- `glm`
- `iv`
- `ols`
- `onehot`
- `panel`
- `survival-aalen`
- `survival-family`
- `survival-parametric`
- `var`

## Ratios that moved

Read against `bench/nightly/ratios.csv` by `tools/nightly_series.py`, under the thresholds that script sets. A ratio moves when either side of it does: read its baseline before calling a movement a regression.

No ratio stepped past its noise or drifted over ten days tonight.
