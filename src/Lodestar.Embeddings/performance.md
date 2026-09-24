# Performance — Lodestar.Embeddings

What `Lodestar.Embeddings` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## SentencePiece and WordPiece encode, against Microsoft.ML.Tokenizers (issue #713)

Full method, and the check that both sides return the same ids:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0 runs of `TokenizerIncumbentBenchmarks`, **default
job**, on 2026-09-13, encoding all 5 000 documents of the corpus per operation.
Microsoft.ML.Tokenizers 2.0.0. `spiece_30k.model` is a unigram model.

| Model | Lodestar | Microsoft.ML.Tokenizers | Allocated, Lodestar | Allocated, Microsoft.ML.Tokenizers |
| --- | ---: | ---: | ---: | ---: |
| [`WordPieceTokenizer`](../../docs/reference/embeddings/tokenization/wordpiecetokenizer.md) | **20.26 ms** | 29.96 ms | 8.71 MB | **3.55 MB** |
| [`SentencePieceTokenizer`](../../docs/reference/embeddings/tokenization/sentencepiecetokenizer.md) | **26.21 ms** | 29.51 ms | 5.44 MB | **3.09 MB** |

**Both encoders run ahead of the incumbent**, and the ids agree with it over the whole corpus. Each
allocates more than the incumbent does: what is left is the result lists, the normalized text and,
for WordPiece, the lowercased copy — a matched token is the vocabulary's own string rather than a
new one.

Both encoders find every piece starting at a position in one walk of a double-array trie, rather
than probing a hash table once per candidate substring.

**What the trie costs the loader, which decision 0004 makes the product.** Measured with a
`Stopwatch` over the same two vocabularies on the same machine, not by BenchmarkDotNet:

| Constructor | First call | Warm |
| --- | ---: | ---: |
| `new SentencePieceTokenizer(vocabulary)` | 35.1 ms | 5.6–7.3 ms |
| `new WordPieceTokenizer(vocabulary)` | 31.8 ms | 6.6–8.1 ms |

Construction is **6–7 ms warm and about 35 ms on the first call in a process**, paid once per
tokenizer; where the first call's extra time goes was not measured apart. Reading
`spiece_30k.model` itself takes 14 ms on the same run.

## Byte-level BPE encode, against its unigram baseline and Microsoft.ML.Tokenizers (issue #673)

Full method, and the check that both sides return the same ids:
[`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md#15-against-the-net-incumbents-issue-438).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 1 CPU, 16 logical and 8 physical cores
(BenchmarkDotNet's own header), Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 runtime,
AVX-512. Window: `BenchmarkDotNet` 0.14.0, **default job**, on 2026-09-14, of `BpeBenchmarks`,
`BpeScalingBenchmarks` and `TokenizerIncumbentBenchmarks`. Every document of the corpus is encoded
per operation, with `tokenizer_30k_bpe.json`'s byte-level model. Microsoft.ML.Tokenizers 2.0.0.

| Row | Mean | Allocated |
| --- | ---: | ---: |
| [`BpeTokenizer`](../../docs/reference/embeddings/tokenization/bpetokenizer.md) | **58.33 ms** | **28.47 MB** |
| `SentencePieceTokenizer`, the `Unigram` baseline | 17.34 ms | 5.43 MB |
| `CodeGenTokenizer`, Microsoft.ML.Tokenizers | 148.73 ms | 59.08 MB |
| Lodestar against it, `TokenizerIncumbentBenchmarks` | **57.95 ms, 2.57× faster** | |

**Byte-level BPE is 2.57× faster than the incumbent**, and 3.36× the cost of this package's own
unigram baseline, which is the cheaper model rather than the faster implementation. The ids agree
with the incumbent's over the whole corpus.
[Decision 0005](../../docs/decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md)
was taken on these numbers.

**The largest single step was a hash function.** The merge ranks were a `Dictionary<long, int>`
keyed by the two ids packed into a `long`, and `long.GetHashCode` is the exclusive or of the two
halves, so every pair whose ids share an exclusive or collided — `(1, 2)` with `(2, 1)`, `(0, 3)`
among them. An open-addressing table with a Fibonacci hash removed two thirds of what was left.

**What compiling the split pattern costs.** Matching it over the corpus takes 17 ms interpreted and
5 compiled. Code generation moves to the tokenizer's first encode, about 2 ms, measured with a
`Stopwatch`; building the tokenizer stays at 5 to 6 ms warm either way. The `netstandard2.0` build
compiles it too, and that cost was not measured on .NET Framework.

**One token with no split point**, `BpeScalingBenchmarks`:

| Length | Mean | Allocated |
| ---: | ---: | ---: |
| 512 | 10.20 μs | 7.48 KB |
| 1,024 | 21.49 μs | 14.53 KB |
| 2,048 | 48.49 μs | 28.58 KB |
| 4,096 | 152.17 μs | 56.63 KB |

The step from 2,048 to 4,096 costs 3.1×, where the others cost about 2×. It is one step, not a
curve: a `Stopwatch` taken further measured each doubling from 4,096 to 32,768 characters at 2.0
to 2.2×.

## [`VectorMath.Dot`](../../docs/reference/embeddings/search/vectormath-dot.md) against `TensorPrimitives`, by vector width (issue #754)

`tensor-primitives` (`bench/README.md` section 14) on the machine below, pinned to one core, three
conditions interleaved over five runs of nine. Median of the five medians, in ms, over
10,000 × 384 floats. Ratios above 1 mean `TensorPrimitives` is faster.

| row | `Vector512` on | `Vector512` off | AVX-512 off |
| --- | ---: | ---: | ---: |
| `ours_dot_knn` | 0.863 | 1.232 | 0.955 |
| `tp_dot_knn` | 6.222 | 3.860 | 3.946 |
| `ours_cosine_knn` | 1.803 | 2.055 | 1.814 |
| `tp_cosine_knn` | 10.795 | 6.130 | 6.363 |
| `ours_one_sweep` | 0.563 | 0.545 | 0.351 |
| `tp_one_sweep` | 0.456 | 0.387 | 0.369 |
| `index_search` | 3.734 | 3.787 | 3.692 |
| **dot ratio** | **0.14** | **0.32** | **0.24** |
| **cosine ratio** | **0.17** | **0.34** | **0.29** |

**Our kernel is ahead on the kNN pattern at every width**, 3–7×, and one long sweep is at parity.
Turning the 512-bit path off halves `TensorPrimitives`' time without changing the direction.
[Decision 0004](../../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)
records it against the hosted runner's opposite reading in
[0004](../../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md).

Machine: AMD Ryzen 7 8700G w/ Radeon 780M Graphics, 16 logical and 8 physical cores, Ubuntu 26.04.1
LTS, .NET 10.0.12 runtime. 2026-09-16.

## Saving and loading an index, against numpy

The nightly's `compare-persistence` of 2026-09-12 has Lodestar at 0.13–0.34× numpy on every
uncompressed index row. **Most of that is not code on either side: it is whose allocator keeps a
freed 15–20 MB block's pages.** glibc raises its mmap threshold after the first free of a block
that size, so a `Harness.Measure` loop of `np.load` refills warm pages; the .NET GC decommits a
dead large-object region, so every iteration of the C# loop page-faults its 15 MB again.

**Conditions.** AMD Ryzen 7 8700G, 8 cores / 16 threads, Ubuntu 26.04.1, .NET 10.0.12, numpy
2.5.3 on Python 3.12, a workstation shared with other sessions under the machine lock, one-minute
load average 2.2–4.1. The probes are a `Harness.Measure`-shaped Stopwatch loop, median of five
rounds, and are not BenchmarkDotNet; read their ratios.

Each side, with its allocator's retention turned off or on:

| row | default | retention flipped | flip |
| --- | ---: | ---: | --- |
| `np.load`, `BytesIO` | 1.073 ms | 7.276 ms | `MALLOC_MMAP_THRESHOLD_=65536`, pages fresh |
| `np.load`, file | 1.728 ms | 2.798 ms | the same |
| [`EmbeddingIndex.Load`](../../docs/reference/embeddings/search/embeddingindex-load.md), memory | 4.409 ms | 2.236 ms | `DOTNET_GCRetainVM=1`, pages kept |
| [`EmbeddingIndex.Load`](../../docs/reference/embeddings/search/embeddingindex-load.md), `MemoryStream` | 5.207 ms | 3.177 ms | the same |
| [`EmbeddingIndex.Load`](../../docs/reference/embeddings/search/embeddingindex-load.md), file | 6.484 ms | 4.348 ms | the same |
| 15.36 MB `memcpy` into a fresh `float[]` | 4.531 ms | 1.239 ms | the same |
| the save row's own `new MemoryStream(capacity)` plus one `memcpy`, no library code | 3.911 ms | 1.543 ms | the same |

- **numpy pays 6.8× on its load the moment its pages are fresh**, and a `memcpy` into a fresh
  `float[]` costs 3.7× one into kept pages. A caller who loads one index once pays those faults
  on both sides; only a loop of loads collects the difference, and the harness is a loop.
- **The save rows charge the C# side for the harness's sink.** `new MemoryStream(indexArtifact.Length)`
  allocates and zeroes 20.6 MB inside the timed window: 3.911 ms of the 4.057 ms
  `embedding_index_save` measures here, against 0.568 ms for the whole chunked base64 encode.
  `io.BytesIO()` grows on pages glibc kept.
- **What remains with retention on both sides is the format's own work**, which
  [decision 0001](../../docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md)
  already prices: a JSON scan of the 20.6 MB document with its 10 000 ids (0.581 ms), a base64
  decode (0.760 ms into warm pages), and the finite scan numpy does not promise (0.235 ms).
  `embedding_index_ingest_npy` stays the like-for-like row.

Two levers inside that remainder were measured and **not taken**: a `Vector512` exponent-bits
finite scan, 0.235 to 0.167 ms, is 1.5% of a load; decoding and scanning in L2-sized slices,
3.711 to 3.655 ms, is noise.

## Compressing an index (issue #378)

The artifact is base64 inside JSON, which spends eight bits to carry six, so it is about 1.33× the
raw block. Deflate takes that back almost exactly — base64 is the one expansion a general-purpose
coder undoes perfectly. The question is what the time costs.

Machine: Intel Core i7-4770S, .NET 10.0.10, a synthetic 4,000 × 384 index through the real `Save`
and `Load`. Window: warmed, median of 7, the five modes interleaved in one window, published
2026-08-20.

| | bytes | × size | save | × save | load | × load |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| plain | 8 231 006 | 1.000 | 10.2 ms | 1.00 | 7.9 ms | 1.00 |
| gzip `Fastest` | 6 257 079 | 0.760 | 270.8 ms | **26.67×** | 56.5 ms | 7.19× |
| gzip `Optimal` | 6 151 764 | 0.747 | 382.1 ms | **37.62×** | 46.6 ms | 5.92× |
| brotli `Fastest` | 6 074 449 | **0.738** | 37.4 ms | 3.68× | 40.8 ms | 5.19× |
| brotli `Optimal` | 6 069 780 | 0.737 | 122.1 ms | 12.02× | 38.3 ms | 4.87× |

**The size claim holds exactly**: 0.747 × 1.333 = 0.996 of the raw block. Level 9 gives the same
bytes as level 6, so there is nothing to tune, and **deflate is dominated on all three axes** —
brotli `Fastest` is smaller than gzip `Optimal` and writes it seven times faster.

**Against numpy, compressed**, from the nightly's own rows at 10,000 × 384 on a hosted runner —
ratios only, per that page's warning, since a shared runner's absolutes are not comparable:

| operation | C# cpu | bytes | Python cpu | Python bytes |
| --- | ---: | ---: | ---: | ---: |
| `embedding_index_save` | 5.949 ms | 20 589 007 | 1.337 ms | 15 360 128 |
| `embedding_index_save_gzip` | 456.995 ms | 15 251 458 | 638.992 ms | 14 022 374 |
| `embedding_index_load` | 5.519 ms | 20 589 007 | 1.327 ms | 15 360 128 |
| `embedding_index_load_gzip` | 81.774 ms | 15 251 458 | 72.368 ms | 14 022 374 |

- **Compressed, this package is ahead of numpy on the write and level with it on the read** —
  1.40× on `savez_compressed`, 0.88× coming back. The deflate coder is the same on both sides, so
  what is compared is what each side hands it.
- **Compression closes most of the format gap.** Uncompressed the artifact is 1.34× numpy's block,
  the expansion
  [decision 0001](../../docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md)
  priced; compressed it is 1.09×.
- **The price grows with the artifact**, which is the opposite of what would make it worth paying:
  0.741× the size for 76.8× the save and 14.8× the load on the corpus, against 37.62× and 5.92× for
  `Optimal` on the 8 MB synthetic index. The indexes big enough for 26 % of a disk to matter are
  the ones where compressing costs most.

## Batched embedding — what the number is, and what it is not

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*BatchEmbedding*' --inProcess
```

**Read this before quoting the ratio.** The model is `tiny_embedder.onnx`: one
Gather node over a 64 × 4 table, because weights are never committed
(`CONTRIBUTING.md`) and a real encoder is a hundred megabytes. Its arithmetic is
free. So what is measured is the per-sequence cost that batching removes — graph
dispatch, thread-pool wake-up, tensor wrapping — and none of the matrix
multiplication a real encoder adds to *both* sides. This is an upper bound on the
speed-up, not the speed-up.

Machine: Intel Core i7-4770S (Haswell, 4 physical cores), Ubuntu 24.04, .NET 10.0.10, X64 RyuJIT
AVX2. Window: four runs in one window — two per target, alternating — published 2026-08-06; the
run's own date was not recorded. Full job, `[MemoryDiagnoser]`, `InProcessEmitToolchain`. Corpus
of 1 to 61 words per text, sub-batch 8. `UnitLoop` is the baseline — one `Embed`
call per text, which is what the guide's three lines amounted to before
`EmbedBatch` existed. **Two runs, both shown**, because BenchmarkDotNet's `±`
describes dispersion inside one process and not reproducibility across processes.

| Texts | `UnitLoop` | `EmbedBatch` | ratio | `EmbedBatchBucketed` | ratio | allocated vs baseline |
| ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 1 | 9.9 / 11.0 µs | 10.6 / 10.8 µs | 1.06 / 0.99 | 10.0 / 11.1 µs | 1.00 / 1.01 | 1.12 |
| 8 | 168 / 180 µs | 105 / 107 µs | 0.62 / 0.60 | 101 / 106 µs | 0.60 / 0.59 | 0.95 |
| 32 | 628 / 672 µs | 360 / 381 µs | 0.57 / 0.57 | 346 / 349 µs | 0.55 / 0.52 | 0.94 / 0.91 |
| 128 | 2 439 / 2 602 µs | 1 482 / 1 460 µs | 0.61 / 0.56 | 1 370 / 1 356 µs | 0.56 / 0.52 | 0.94 / 0.91 |

**Batching removes about 40 % of the wall clock** from 8 texts upward and stays
there — 0.56–0.62 across every pairing — as the per-call overhead is amortized
over the whole sub-batch. At a single text there is nothing to amortize and the
two paths are a wash: 1.06 in one run, 0.99 in the next, which is a way of
saying this benchmark cannot tell them apart there rather than that either wins.

**Bucketing is a different story, and the honest answer is smaller.** It engages
only when the corpus spans more than one sub-batch, so the rows at 1 and 8 above
run the *identical* code in both columns — they are the control, and what they
differ by is this harness's noise floor: 1–3 % on seven of the eight control
measurements taken here, with one outlier at 5.7 %. At 128 texts bucketing is
ahead by 5.8–7.5 % in all four pairings, and ahead at 32 in all four as well.
The sign is consistent where the magnitude alone would not be decisive. What is
decisive is the allocation column, which is counted rather than sampled:
1 764 KB → 1 697 KB at 128 texts, in every run. That is padding genuinely not
written. On a model doing real work that padding would be matrix multiplication
not performed, and the time would follow; this model cannot show it, so the
claim stops here.

**The two builds are level here, and that is the structurally correct answer.** The same benchmark
against the `netstandard2.0` assemblies puts that side 1.4–5.7 % *ahead* of net10 in all four
pairings (355 / 365 µs against 360 / 381 at 32 texts; 1 418 / 1 419 against 1 482 / 1 460 at 128) —
inside the harness's noise, but consistent in direction. `Pooling` guards its `Vector<T>` branch
with `accumulator.Length >= Vector<float>.Count`, and `tiny_embedder.onnx` has a hidden size of 4
(`EMBEDDING_DIM` in `tools/build_tiny_models.py`) against `Vector<float>.Count` of 8 under AVX2, so
on net10 the guard is false and both builds run the same scalar tail loop. This benchmark cannot
resolve a difference between the targets, and reports that instead of a number. Where the vector
path does engage it is worth 4×–7×, in `VectorMath` over 384–1024 dimensions
([`bench/README.md`](https://github.com/CyrilB1531/lodestar/blob/main/bench/README.md) section 2).
The one difference it does resolve is counted rather than timed: the unit-loop path allocates 0.6 %
more on `netstandard2.0` (1 887 KB against 1 875 at 128 texts), identically in both runs, while the
two batch paths allocate byte for byte the same on both targets.

```bash
dotnet run -c Release --project bench/Lodestar.NetStandard.Benchmarks -- --filter '*BatchEmbedding*'
```

`--inProcess` on the first command and not on the second is the point. The `netstandard2.0` project
pins `InProcessEmitToolchain` in its `Program.cs` — it has to, or BenchmarkDotNet's generated
project re-resolves the `ProjectReference` and silently restores the net10.0 build — so the flag is
what puts the net10 side on the same toolchain. Without it the two commands measure the same code
two different ways.

**Conditions.** The four runs behind the table were taken back to back in one window, alternating
net10 and `netstandard2.0`, with the one-minute load average between 5.1 and 5.9 on 8 logical cores
— the editor's language servers and the session driving the runs are part of that load and cannot
be excluded from inside it. Both columns pay it equally, so the table is internally comparable; it
is not comparable to figures taken on this machine in a quieter state, and the ratios travel
between such sets while the absolute microseconds do not.
