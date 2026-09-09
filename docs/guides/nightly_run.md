# Nightly benchmark run

<!-- nightly-baseline: 04d24cf8806c078a1d9ffa71e8bc3f5cdb902c31 -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `04d24cf8806c078a1d9ffa71e8bc3f5cdb902c31`
- Previous run: `04d24cf8806c078a1d9ffa71e8bc3f5cdb902c31`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run:

- `BatchEmbeddingBenchmarks`
- `BpeBenchmarks`
- `BpeScalingBenchmarks`
- `DecompositionBenchmarks`
- `PersistenceBenchmarks`
- `TokenizerIncumbentBenchmarks`

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method             | CorpusSize | Mean         | Error       | StdDev     | Ratio | RatioSD | Gen0    | Gen1   | Allocated  | Alloc Ratio |
|------------------- |----------- |-------------:|------------:|-----------:|------:|--------:|--------:|-------:|-----------:|------------:|
| **UnitLoop**           | **1**          |     **4.479 μs** |   **2.5105 μs** |  **0.1376 μs** |  **1.00** |    **0.04** |  **0.1526** |      **-** |     **2.6 KB** |        **1.00** |
| EmbedBatch         | 1          |     4.564 μs |   0.4006 μs |  0.0220 μs |  1.02 |    0.03 |  0.1831 |      - |       3 KB |        1.15 |
| EmbedBatchBucketed | 1          |     4.540 μs |   0.2531 μs |  0.0139 μs |  1.01 |    0.03 |  0.1831 |      - |       3 KB |        1.15 |
|                    |            |              |             |            |       |         |         |        |            |             |
| **UnitLoop**           | **8**          |    **67.411 μs** |   **2.2786 μs** |  **0.1249 μs** |  **1.00** |    **0.00** |  **5.7373** | **0.1221** |   **94.76 KB** |        **1.00** |
| EmbedBatch         | 8          |    43.438 μs |   2.0113 μs |  0.1102 μs |  0.64 |    0.00 |  5.3711 | 0.2441 |   87.78 KB |        0.93 |
| EmbedBatchBucketed | 8          |    43.505 μs |   5.0513 μs |  0.2769 μs |  0.65 |    0.00 |  5.3711 | 0.2441 |   87.78 KB |        0.93 |
|                    |            |              |             |            |       |         |         |        |            |             |
| **UnitLoop**           | **32**         |   **256.032 μs** |  **19.6819 μs** |  **1.0788 μs** |  **1.00** |    **0.01** | **20.0195** | **0.4883** |  **334.02 KB** |        **1.00** |
| EmbedBatch         | 32         |   153.079 μs |  26.7292 μs |  1.4651 μs |  0.60 |    0.01 | 18.5547 | 1.2207 |  306.63 KB |        0.92 |
| EmbedBatchBucketed | 32         |   141.042 μs |  12.1876 μs |  0.6680 μs |  0.55 |    0.00 | 17.8223 | 0.9766 |  293.12 KB |        0.88 |
|                    |            |              |             |            |       |         |         |        |            |             |
| **UnitLoop**           | **128**        | **1,086.498 μs** | **914.0061 μs** | **50.0998 μs** |  **1.00** |    **0.06** | **80.0781** | **3.9063** | **1336.03 KB** |        **1.00** |
| EmbedBatch         | 128        |   600.585 μs |  78.8469 μs |  4.3219 μs |  0.55 |    0.02 | 74.2188 | 9.7656 | 1225.67 KB |        0.92 |
| EmbedBatchBucketed | 128        |   558.454 μs |  66.6907 μs |  3.6555 μs |  0.51 |    0.02 | 70.3125 | 9.7656 | 1158.15 KB |        0.87 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method  | Mean     | Error    | StdDev  | Ratio | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|--------:|------:|----------:|----------:|------------:|
| Unigram | 264.3 ms |  8.48 ms | 0.46 ms |  1.00 | 1500.0000 |  30.32 MB |        1.00 |
| Bpe     | 441.7 ms | 33.19 ms | 1.82 ms |  1.67 | 7000.0000 | 112.18 MB |        3.70 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                    | Length | Mean      | Error     | StdDev   | Gen0   | Gen1   | Allocated |
|-------------------------- |------- |----------:|----------:|---------:|-------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |  **79.94 μs** |  **2.432 μs** | **0.133 μs** | **1.2207** |      **-** |  **20.38 KB** |
| **BpeOnOnePathologicalToken** | **1024**   | **166.82 μs** | **17.267 μs** | **0.946 μs** | **2.4414** |      **-** |  **39.93 KB** |
| **BpeOnOnePathologicalToken** | **2048**   | **370.53 μs** | **28.425 μs** | **1.558 μs** | **4.3945** |      **-** |  **78.98 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **829.09 μs** | **31.747 μs** | **1.740 μs** | **8.7891** | **0.9766** | **157.03 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.DecompositionBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.4 LTS (Noble Numbat)
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
| TruncatedSvd_Rank20                       |  22.51 ms |  1.595 ms | 0.087 ms |  1.00 |    0.00 |
| Nmf_Rank20                                | 160.38 ms | 13.987 ms | 0.767 ms |  7.12 |    0.04 |
| MlNet_ProjectToPrincipalComponents_Rank20 |  18.88 ms |  2.385 ms | 0.131 ms |  0.84 |    0.01 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method                 | Mean     | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|----------------------- |---------:|----------:|----------:|---------:|---------:|---------:|----------:|
| VocabTxt               | 3.585 ms | 2.0775 ms | 0.1139 ms | 117.1875 | 109.3750 |  39.0625 |   3.62 MB |
| TokenizerJsonWordPiece | 8.865 ms | 4.6700 ms | 0.2560 ms | 187.5000 | 171.8750 |  46.8750 |   5.72 MB |
| TokenizerJsonUnigram   | 8.842 ms | 0.6746 ms | 0.0370 ms |  93.7500 |  78.1250 |  31.2500 |   4.64 MB |
| SpieceModel            | 3.023 ms | 2.2558 ms | 0.1236 ms | 121.0938 | 113.2813 |  39.0625 |   3.36 MB |
| TfidfSave              | 1.594 ms | 0.4065 ms | 0.0223 ms |  29.2969 |  23.4375 |  23.4375 |   2.09 MB |
| TfidfLoad              | 3.634 ms | 1.2382 ms | 0.0679 ms |  85.9375 |  78.1250 |  23.4375 |   2.86 MB |
| EmbeddingIndexSave     | 3.178 ms | 0.2422 ms | 0.0133 ms | 285.1563 | 281.2500 | 281.2500 |  19.87 MB |
| EmbeddingIndexLoad     | 3.974 ms | 0.4975 ms | 0.0273 ms | 203.1250 | 171.8750 | 140.6250 |  15.72 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

```text
BenchmarkDotNet v0.14.0, Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.12 (10.0.1226.42308), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3
```

<!-- markdownlint-disable MD060 -->

| Method       | Model         | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0      | Gen1     | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|----------:|---------:|------:|--------:|----------:|---------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **51.79 ms** | **25.375 ms** | **1.391 ms** |  **1.00** |    **0.03** | **4200.0000** | **100.0000** |  **68.25 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  42.26 ms |  0.924 ms | 0.051 ms |  0.82 |    0.02 |  166.6667 |        - |   3.55 MB |        0.05 |
|              |               |           |           |          |       |         |           |          |           |             |
| **Lodestar**     | **SentencePiece** | **280.42 ms** | **25.823 ms** | **1.415 ms** |  **1.00** |    **0.01** | **1500.0000** |        **-** |  **30.33 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  40.84 ms |  1.563 ms | 0.086 ms |  0.15 |    0.00 |  153.8462 |        - |   3.09 MB |        0.10 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `persistence`

### compare-persistence

```text
Python: {'tokenizers': '0.23.1', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.0', 'numpy': '2.5.1'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.12
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 4.518 | 7.647 | 1.69x | 4.814 | 7.647 | 1.59x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 9.702 | 12.903 | 1.33x | 10.027 | 12.903 | 1.29x | 706,526 | 706,526 |
| tokenizer_json_unigram | 9.541 | 32.821 | 3.44x | 9.851 | 32.819 | 3.33x | 1,990,038 | 1,990,038 |
| spiece_model | 3.972 | 22.972 | 5.78x | 4.159 | 22.970 | 5.52x | 533,084 | 533,084 |
| tfidf_save | 1.371 | 1.861 | 1.36x | 1.389 | 1.861 | 1.34x | 581,787 | 591,922 |
| tfidf_load | 4.613 | 3.292 | 0.71x | 6.704 | 3.291 | 0.49x | 581,787 | 591,922 |
| embedding_index_save | 2.973 | 1.272 | 0.43x | 3.143 | 1.272 | 0.40x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 100.140 | 64.809 | 0.65x | 8.754 | 3.954 | 0.45x | 20,589,007 | 15,360,128 |
| embedding_index_load | 3.825 | 1.311 | 0.34x | 4.274 | 1.311 | 0.31x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 4.421 | 0.833 | 0.19x | 4.791 | 0.832 | 0.17x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 2.593 | 1.365 | 0.53x | 2.779 | 1.365 | 0.49x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.065 | 1.390 | 1.30x | 1.247 | 1.389 | 1.11x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.000 | 83.55x | 0.000 | 0.000 | 83.55x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 354.133 | 496.316 | 1.40x | 355.953 | 496.273 | 1.39x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 63.091 | 56.555 | 0.90x | 64.416 | 56.550 | 0.88x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->
