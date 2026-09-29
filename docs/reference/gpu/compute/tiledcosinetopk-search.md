# TiledCosineTopK.Search

The best *k* rows for each query in a batch, best first.

<!-- docs-declaration -->

```csharp
public IReadOnlyList<IReadOnlyList<SearchResult>> Search(DeviceEmbeddingMatrix matrix, ReadOnlySpan<float> queries, int queryCount, int k)
```

**Parameters** — `matrix` is the resident matrix to sweep. `queries` holds `queryCount` ×
`matrix.Dimension` values, row-major, normalized on upload as the matrix rows were. `k` is how many
hits per query; fewer come back when the matrix is smaller.

**Returns** — one list per query, in the batch's own order.

**Exceptions** — `ArgumentNullException` when `matrix` is null; `ArgumentOutOfRangeException` when
`queryCount` or `k` is below 1; `ObjectDisposedException` when `matrix`, or the context it and the
kernel share, was disposed; `ArgumentException` when `queries` is not exactly the batch or holds a
`NaN` or an infinity, or when `matrix` was uploaded to another context than the kernel's.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Embeddings.Search;
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var matrix = DeviceEmbeddingMatrix.Upload(
    context, [1f, 0f, 0f, 1f], count: 2, dimension: 2);
var kernel = new TiledCosineTopK(context);

IReadOnlyList<IReadOnlyList<SearchResult>> hits = kernel.Search(matrix, [0f, 1f], 1, 1);

int best = hits[0][0].Index;  // => 1
```

**Remarks** — **the batch is what makes this worth doing.** One query against a hundred thousand
rows measured 6.6× faster than the SIMD path; two hundred and fifty-six queries against the same
rows measured 13.2×, because a launch and a read-back are amortised across the batch. A caller
with one query at a time is better served by [`EmbeddingIndex.Search`](../../embeddings/search/embeddingindex-search.md).

Query transfer and result read-back are per call; the matrix is not. Five device buffers are
allocated per call and freed before it returns: the queries; the scores, one per row and query; the
selection's heaps, one per lane of the group, each of `min(k, ⌈rows / group size⌉)` entries; and
the hits' indices and scores, `min(k, rows)` per query each. The heaps can outnumber the scores: a
one-row matrix swept by a 256-wide group holds 256 heap entries against one score. An unnormalized
row can score negative infinity honestly, and is still selected.

**The matrix must have been uploaded to the kernel's own context, and neither may be disposed.**
A buffer is a pointer into one accelerator's memory, so a foreign or released one is refused
before anything launches ([#1214](https://github.com/CyrilB1531/lodestar/issues/1214)).

**A non-finite query is refused rather than scored.** A `NaN` score loses every comparison, so the
selection has no order to rank it by — the argmax before #1214 found no row for a slot and wrote
past its buffer ([#898](https://github.com/CyrilB1531/lodestar/issues/898)).

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledCosineTopK`](tiledcosinetopk.md).
