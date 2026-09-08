# Nightly benchmark run

<!-- nightly-baseline: 3d9d04a8895d8f76b4360c1a3f3bf21a39dea535 -->

> **Generated. Do not edit.** Produced by `.github/workflows/bench-nightly.yml`; every edit is
> overwritten by the next run. The curated figures, measured on a named machine, are in
> [performance](performance). The last known reading for a method quiet tonight is in
> [benchmark_latest](benchmark_latest).

**Read the ratios, not the means.** These run on a GitHub hosted runner: a shared VM whose
hardware differs from night to night and whose neighbours are unknown. An absolute figure here
is not comparable to the performance page, and not reliably comparable to yesterday's. A ratio
against a baseline measured in the same run, on the same VM, in the same minute, is.

## This run

- Commit: `3d9d04a8895d8f76b4360c1a3f3bf21a39dea535`
- Previous run: `3d9d04a8895d8f76b4360c1a3f3bf21a39dea535`
- Runner: Linux / X64 (GitHub hosted)

## Classes re-run

Selected by `tools/select_benchmarks.py` from the sources that changed since the previous run:

- `BatchEmbeddingBenchmarks`
- `BpeBenchmarks`
- `BpeScalingBenchmarks`
- `PersistenceBenchmarks`
- `StatsBenchmarks`
- `TokenizerIncumbentBenchmarks`

### Lodestar.Text.Benchmarks.BatchEmbeddingBenchmarks-report-github

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

| Method             | CorpusSize | Mean         | Error       | StdDev    | Ratio | Gen0    | Gen1   | Allocated  | Alloc Ratio |
|------------------- |----------- |-------------:|------------:|----------:|------:|--------:|-------:|-----------:|------------:|
| **UnitLoop**           | **1**          |     **5.670 μs** |   **0.6641 μs** | **0.0364 μs** |  **1.00** |  **0.1526** |      **-** |     **2.6 KB** |        **1.00** |
| EmbedBatch         | 1          |     5.856 μs |   0.1078 μs | 0.0059 μs |  1.03 |  0.1831 |      - |       3 KB |        1.15 |
| EmbedBatchBucketed | 1          |     5.905 μs |   0.6907 μs | 0.0379 μs |  1.04 |  0.1831 |      - |       3 KB |        1.15 |
|                    |            |              |             |           |       |         |        |            |             |
| **UnitLoop**           | **8**          |    **88.094 μs** |   **3.6339 μs** | **0.1992 μs** |  **1.00** |  **5.7373** | **0.1221** |   **94.76 KB** |        **1.00** |
| EmbedBatch         | 8          |    53.874 μs |   6.7032 μs | 0.3674 μs |  0.61 |  5.3711 | 0.2441 |   87.78 KB |        0.93 |
| EmbedBatchBucketed | 8          |    53.812 μs |   3.3760 μs | 0.1850 μs |  0.61 |  5.3711 | 0.2441 |   87.78 KB |        0.93 |
|                    |            |              |             |           |       |         |        |            |             |
| **UnitLoop**           | **32**         |   **329.989 μs** |  **28.7289 μs** | **1.5747 μs** |  **1.00** | **20.0195** | **0.4883** |  **334.02 KB** |        **1.00** |
| EmbedBatch         | 32         |   196.268 μs |  50.6999 μs | 2.7790 μs |  0.59 | 18.5547 | 1.2207 |  306.63 KB |        0.92 |
| EmbedBatchBucketed | 32         |   182.593 μs |  26.4331 μs | 1.4489 μs |  0.55 | 17.8223 | 0.9766 |  293.12 KB |        0.88 |
|                    |            |              |             |           |       |         |        |            |             |
| **UnitLoop**           | **128**        | **1,325.540 μs** |  **38.3684 μs** | **2.1031 μs** |  **1.00** | **80.0781** | **3.9063** | **1336.03 KB** |        **1.00** |
| EmbedBatch         | 128        |   784.926 μs | 154.0709 μs | 8.4451 μs |  0.59 | 74.2188 | 9.7656 | 1225.67 KB |        0.92 |
| EmbedBatchBucketed | 128        |   685.269 μs |  34.1684 μs | 1.8729 μs |  0.52 | 70.3125 | 9.7656 | 1158.15 KB |        0.87 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeBenchmarks-report-github

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

| Method  | Mean     | Error    | StdDev  | Ratio | Gen0      | Allocated | Alloc Ratio |
|-------- |---------:|---------:|--------:|------:|----------:|----------:|------------:|
| Unigram | 337.9 ms | 19.29 ms | 1.06 ms |  1.00 | 1000.0000 |  30.32 MB |        1.00 |
| Bpe     | 560.6 ms | 49.29 ms | 2.70 ms |  1.66 | 7000.0000 | 112.18 MB |        3.70 |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.BpeScalingBenchmarks-report-github

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

| Method                    | Length | Mean       | Error    | StdDev  | Gen0   | Allocated |
|-------------------------- |------- |-----------:|---------:|--------:|-------:|----------:|
| **BpeOnOnePathologicalToken** | **512**    |   **103.2 μs** |  **5.14 μs** | **0.28 μs** | **1.2207** |  **20.38 KB** |
| **BpeOnOnePathologicalToken** | **1024**   |   **221.0 μs** |  **5.08 μs** | **0.28 μs** | **2.4414** |  **39.93 KB** |
| **BpeOnOnePathologicalToken** | **2048**   |   **497.5 μs** | **42.76 μs** | **2.34 μs** | **3.9063** |  **78.98 KB** |
| **BpeOnOnePathologicalToken** | **4096**   | **1,017.8 μs** | **78.78 μs** | **4.32 μs** | **7.8125** | **157.03 KB** |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.PersistenceBenchmarks-report-github

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

| Method                 | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated |
|----------------------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
| VocabTxt               |  4.534 ms | 2.4593 ms | 0.1348 ms | 117.1875 | 109.3750 |  39.0625 |   3.62 MB |
| TokenizerJsonWordPiece | 10.753 ms | 3.0641 ms | 0.1680 ms | 156.2500 | 125.0000 |  31.2500 |   5.72 MB |
| TokenizerJsonUnigram   | 11.159 ms | 0.6388 ms | 0.0350 ms |  93.7500 |  78.1250 |  31.2500 |   4.64 MB |
| SpieceModel            |  4.014 ms | 4.5457 ms | 0.2492 ms | 109.3750 | 101.5625 |  31.2500 |   3.36 MB |
| TfidfSave              |  1.926 ms | 0.2695 ms | 0.0148 ms |  29.2969 |  23.4375 |  23.4375 |   2.09 MB |
| TfidfLoad              |  4.517 ms | 0.9554 ms | 0.0524 ms |  85.9375 |  78.1250 |  23.4375 |   2.86 MB |
| EmbeddingIndexSave     |  3.784 ms | 0.4495 ms | 0.0246 ms | 285.1563 | 281.2500 | 281.2500 |  19.87 MB |
| EmbeddingIndexLoad     |  4.773 ms | 0.5892 ms | 0.0323 ms | 203.1250 | 171.8750 | 140.6250 |  15.72 MB |

<!-- markdownlint-enable MD060 -->

### Lodestar.Text.Benchmarks.TokenizerIncumbentBenchmarks-report-github

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

| Method       | Model         | Mean      | Error    | StdDev   | Ratio | Gen0      | Gen1     | Allocated | Alloc Ratio |
|------------- |-------------- |----------:|---------:|---------:|------:|----------:|---------:|----------:|------------:|
| **Lodestar**     | **WordPiece**     |  **62.86 ms** | **7.502 ms** | **0.411 ms** |  **1.00** | **4250.0000** | **125.0000** |  **68.25 MB** |        **1.00** |
| MlTokenizers | WordPiece     |  53.13 ms | 1.907 ms | 0.105 ms |  0.85 |  200.0000 |        - |   3.55 MB |        0.05 |
|              |               |           |          |          |       |           |          |           |             |
| **Lodestar**     | **SentencePiece** | **352.49 ms** | **8.565 ms** | **0.469 ms** |  **1.00** | **1000.0000** |        **-** |  **30.33 MB** |        **1.00** |
| MlTokenizers | SentencePiece |  52.06 ms | 0.814 ms | 0.045 ms |  0.15 |  100.0000 |        - |   3.09 MB |        0.10 |

<!-- markdownlint-enable MD060 -->

## Against rapidfuzz, in this same run

Both sides on this VM in these minutes, which is what makes the ratio readable where the absolutes are not.

- `persistence`

### compare-persistence

```text
Python: {'tokenizers': '0.23.1', 'sentencepiece': '0.2.2', 'scikit-learn': '1.9.0', 'numpy': '2.5.1'} (py 3.12.14)
C#:     Lodestar on .NET 10.0.11
```

<!-- markdownlint-disable MD060 -->

| operation | C# ms | Py ms | wall | C# cpu | Py cpu | cpu | C# bytes | Py bytes |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| vocab_txt | 5.871 | 9.744 | 1.66x | 6.096 | 9.743 | 1.60x | 228,891 | 228,891 |
| tokenizer_json_wordpiece | 12.233 | 16.402 | 1.34x | 12.587 | 16.401 | 1.30x | 706,526 | 706,526 |
| tokenizer_json_unigram | 12.424 | 37.009 | 2.98x | 12.622 | 37.006 | 2.93x | 1,990,038 | 1,990,038 |
| spiece_model | 5.075 | 30.411 | 5.99x | 5.308 | 30.407 | 5.73x | 533,084 | 533,084 |
| tfidf_save | 1.749 | 2.401 | 1.37x | 1.781 | 2.401 | 1.35x | 581,787 | 591,922 |
| tfidf_load | 5.483 | 4.247 | 0.77x | 5.726 | 4.247 | 0.74x | 581,787 | 591,922 |
| embedding_index_save | 3.820 | 1.326 | 0.35x | 4.038 | 1.326 | 0.33x | 20,589,007 | 15,360,128 |
| embedding_index_save_file | 50.024 | 36.673 | 0.73x | 12.436 | 4.354 | 0.35x | 20,589,007 | 15,360,128 |
| embedding_index_load | 4.659 | 1.395 | 0.30x | 5.004 | 1.395 | 0.28x | 20,589,007 | 15,360,128 |
| embedding_index_load_file | 5.575 | 0.874 | 0.16x | 5.993 | 0.871 | 0.15x | 20,589,007 | 15,360,128 |
| embedding_index_load_memory | 3.547 | 1.508 | 0.43x | 3.808 | 1.508 | 0.40x | 20,589,007 | 15,360,128 |
| embedding_index_ingest_npy | 1.169 | 1.544 | 1.32x | 1.367 | 1.544 | 1.13x | 15,360,128 | 15,360,128 |
| embedding_index_view_floor | 0.000 | 0.001 | 78.18x | 0.000 | 0.001 | 78.18x | 20,589,007 | 15,360,128 |
| embedding_index_save_gzip | 454.242 | 638.432 | 1.41x | 456.594 | 638.367 | 1.40x | 15,250,490 | 14,022,374 |
| embedding_index_load_gzip | 80.478 | 72.676 | 0.90x | 82.153 | 72.676 | 0.88x | 15,250,490 | 14,022,374 |

ratio > 1 means Lodestar is faster. cpu is the honest one: elapsed time
hides work .NET does on background GC threads; CPython is single-threaded.
bytes is what the row wrote or read; a results file from before #378
carries none, and the two columns then disappear rather than read zero.

<!-- markdownlint-enable MD060 -->
