# Performance — Lodestar.Gpu

What `Lodestar.Gpu` costs against the library a reader would otherwise reach for. How to read
a row, and what this page leaves out:
[`docs/guides/performance.md`](../../docs/guides/performance.md#how-to-read-a-row).

## Lodestar.Gpu — four kernels against their CPU paths (issue #444)

Measured 2026-09-10, on the one machine `bench/README.md`'s GPU gate
says a GPU figure has to be published beside:

| | |
| --- | --- |
| GPU | **NVIDIA GeForce RTX 5070 Ti**, 15 877 MB, CUDA SM_120 |
| CPU | AMD Ryzen 7 8700G, 8 physical / 16 logical cores, AVX-512 |
| OS / runtime | Ubuntu 26.04.1 LTS, .NET SDK 10.0.401, .NET 10.0.12 |
| ILGPU | 1.5.3, CUDA backend |
| BenchmarkDotNet job | `--job short` — 3 iterations, 3 warmups, 1 launch |

**Read this before the numbers.** Three caveats travel with every table below.

1. **The baseline is single-threaded.** bench/README.md's GPU gate asks a kernel to be priced against *this
   repository's own path*, and none of those paths is parallel. A `Parallel.For` over eight cores
   would close a large part of every gain here, most of all Myers'. These are honest against the
   gate as written; they are not a claim about a parallel CPU implementation.
2. **`--job short` is three iterations.** The `GpuResident` rows sit at 0.7–7 % standard error,
   which carries a verdict. Some `GpuFromHost` rows do not — one shows a confidence interval wider
   than its own mean — and those are read as orders of magnitude, not as figures.
3. **The first run of these benchmarks measured the wrong device**, and the numbers below are the
   second. `Context.GetPreferredDevice(preferCPU: false)` returned an OpenCL runtime that executes
   on the processor — ILGPU enumerated `cpu-skylake-avx512-AMD Ryzen 7 8700G` as an OpenCL device
   beside the CUDA card — and a check on the accelerator *type* called it a GPU. `GpuContext` now
   orders the devices itself and asks OpenCL for the device type, and the device's name is a
   column in every table so the mistake cannot repeat silently.

### Tiled cosine + top-k, against [`EmbeddingIndex.Search`](../../docs/reference/embeddings/search/embeddingindex-search.md)

384 dimensions, top-10, seeded corpus. `GpuResident` is the gate row: query transfer and result
read-back are inside the measurement, the corpus is not.

| Documents | Queries | `SimdBaseline` | `GpuResident` | gain | `GpuFromHost` |
| ---: | ---: | ---: | ---: | ---: | ---: |
| 10 000 | 1 | 759.4 µs | 219.2 µs | 3.5× | 11.9 ms |
| 100 000 | 1 | 9.32 ms | 1.41 ms | **6.6×** | 86.2 ms |
| 10 000 | 256 | 197.5 ms | 18.4 ms | **10.8×** | 29.9 ms |
| 100 000 | 256 | 2 356.9 ms | 178.2 ms | **13.2×** | 278.6 ms |

**Clears the 5–10× gate at three sizes of four**, and misses at the smallest corpus with a single
query — which is where the crossing point belongs and why the sizes bracket it.

**`GpuFromHost` is the argument for residency, and it is brutal.** At 100 000 documents and one
query it is **9.25× slower than the CPU path**: 147 MB of corpus uploaded to answer one question.
At 256 queries the same upload amortises and the row returns to 8.5× faster than the CPU. Below
roughly a hundred queries per corpus, a GPU package that uploads per call is a pessimisation.

Allocation is the secondary win: 434 KB against the SIMD path's 200 MB at the largest size, a
ratio of 0.002, because the kernel returns *k* hits rather than a score per document.

### Sparse-dense product, against [`CsrMatrix.Multiply`](../../docs/reference/abstractions/sparse/csrmatrix-multiply.md)

20 000 terms at 0.2 % density, double precision on both sides.

| Documents | Width | `CpuBaseline` | `GpuResident` | gain |
| ---: | ---: | ---: | ---: | ---: |
| 5 000 | 64 | 7.92 ms | 4.54 ms | 1.74× |
| 5 000 | 256 | 38.4 ms | 11.1 ms | 3.45× |
| 50 000 | 64 | 71.5 ms | 8.25 ms | **8.66×** |
| 50 000 | 256 | 377.5 ms | 20.0 ms | **18.9×** |

`bench/README.md` predicted this kernel might miss the gate on FP64, since a consumer card runs
double precision at a fraction of single. **That was wrong, and the reason is worth keeping**:
sparse-dense is bound by memory bandwidth, not by the double-precision units, so the reduced FP64
rate never becomes the constraint. It clears the gate at both realistic vectorizer sizes.

The cost it does carry is allocation — **5× the CPU path** at the small sizes, from the staged
upload and the downloaded result.

### Myers bit-parallel edit distance, against [`Levenshtein.Distance`](../../docs/reference/text/distances/levenshtein-distance.md)

24-character pattern, 26-letter alphabet, seeded corpus.

| Texts | Text length | `CpuBaseline` | `GpuResident` | gain | `GpuFromHost`, 2026-09-10 | `GpuFromHost`, 2026-09-17 |
| ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 10 000 | 32 | 1 600.7 µs | 57.4 µs | **27.9×** | 2 943 µs | 421 µs |
| 10 000 | 256 | 7 585.8 µs | 121.6 µs | **62.4×** | 21 277 µs | 1.65 ms |
| 200 000 | 32 | 32.07 ms | 220.2 µs | **145.7×** | 55.5 ms | 5.27 ms |
| 200 000 | 256 | 153.4 ms | 1.19 ms | **129.4×** | 450.0 ms | 37.3 ms |

**This was written expecting a failure and is the largest gain of the four.** The reasoning behind
the prediction — that Myers is bit-parallel on both sides, so the CPU already spends tens of
nanoseconds per short pair — was true and did not settle it: the baseline is *one thread* and the
kernel is tens of thousands, over pairs with no dependency between them. Caveat 1 above bites
hardest here.

`GpuFromHost` runs **4.1× to 6.5× below the CPU path**: renaming the batch is a 64 KB code table on
the host rather than a dictionary probe per character
([#853](https://github.com/CyrilB1531/lodestar/issues/853)). That column was measured on 2026-09-17,
same machine and `--job short`, with a `CpuBaseline` of 1.73 ms, 8.08 ms, 34.4 ms and 162 ms in the
same window; the other columns are the 2026-09-10 run.

### MinHash signatures, against [`MinHash.Signature`](../../docs/reference/text/similarity/minhash-signature.md)

24 tokens per document over a vocabulary of 5 000, seeded corpus, the same coefficients on both
sides. Three rows because the two sides divide the work differently: the CPU path hashes and
minimises in one pass, while the kernel takes hashes the host already computed.

| Documents | Permutations | `CpuBaseline` | `GpuResident` | gain | `GpuWithHashing` | gain |
| ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 5 000 | 64 | 63.2 ms | 0.95 ms | 66× | 49.6 ms | **1.27×** |
| 5 000 | 128 | 79.8 ms | 1.94 ms | 41× | 50.8 ms | **1.57×** |
| 50 000 | 64 | 637.1 ms | 15.1 ms | 42× | 503.2 ms | **1.27×** |
| 50 000 | 128 | 779.6 ms | 23.2 ms | 34× | 537.4 ms | **1.45×** |

**`GpuWithHashing` is the row that matters, and it is the one that misses the gate.** The
minimisation is 34× to 66× faster on the accelerator; a caller starting from tokens sees 1.27× to
1.57×, because hashing is most of the work and it stays on the host. This kernel clears the GPU gate on the part it took and misses it on the part a caller experiences.

Located rather than disappointing: the next move is to hash on the accelerator, which is a separate
kernel because parity requires the first four bytes of SHA-1 little-endian and a device
implementation agreeing bit for bit is its own piece of work. Until then the kernel earns its place
for a caller who **already holds hashes** — sketching one corpus under several permutation sets, for
instance, where hashing is paid once and minimisation many times.

### What residency buys across two operations

Two sparse-dense products; `RoundTripped` is the baseline because the question is what an
intermediate costs when it crosses the bus, not whether the accelerator wins.

| Rows | Width | `RoundTripped` | `Chained` | gain | `CpuBaseline` |
| ---: | ---: | ---: | ---: | ---: | ---: |
| 2 000 | 32 | 1 234.7 µs | 552.6 µs | 2.23× | 1 795.9 µs |
| 2 000 | 128 | 5 372.1 µs | 1 830.1 µs | 2.94× | 8 325.9 µs |
| 20 000 | 32 | 1 990.5 µs | 1 602.4 µs | 1.24× | 6 324.3 µs |
| 20 000 | 128 | 6 288.9 µs | 3 490.4 µs | 1.80× | 25 519.8 µs |

`bench/README.md` predicted the gap would be roughly flat in `Rows`. **It is not — it shrinks**,
2.23× to 1.24× at width 32, and the arithmetic never predicted otherwise: a fixed transfer against
work that grows with `Rows` is a falling *share* of the total. The prediction in `Width` holds.

The reading for a caller inverts the usual intuition: **residency is worth most where the work is
smallest.** A large job amortises a round trip on its own; a small one does not.
