# TiledCosineTopK.Search

The best *k* rows for each query in a batch, best first.

<!-- docs-declaration -->

```csharp
public IReadOnlyList<IReadOnlyList<GpuSearchResult>> Search(DeviceEmbeddingMatrix matrix, ReadOnlySpan<float> queries, int queryCount, int k)
```

**Parameters** — `matrix` is the resident matrix to sweep. `queries` holds `queryCount` ×
`matrix.Dimension` values, row-major, normalized on upload as the matrix rows were. `k` is how many
hits per query; fewer come back when the matrix is smaller.

**Returns** — one list per query, in the batch's own order.

**Exceptions** — `ArgumentNullException` when `matrix` is null; `ArgumentOutOfRangeException` when
`queryCount` or `k` is below 1; `ArgumentException` when `queries` is not exactly the batch.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var matrix = DeviceEmbeddingMatrix.Upload(
    context, [1f, 0f, 0f, 1f], count: 2, dimension: 2);
var kernel = new TiledCosineTopK(context);

IReadOnlyList<IReadOnlyList<GpuSearchResult>> hits = kernel.Search(matrix, [0f, 1f], 1, 1);

int best = hits[0][0].Index;  // => 1
```

**Remarks** — **the batch is what makes this worth doing.** One query against a hundred thousand
rows measured 6.6× faster than the SIMD path; two hundred and fifty-six queries against the same
rows measured 13.2×, because a launch and a read-back are amortised across the batch. A caller
with one query at a time is better served by [`EmbeddingIndex.Search`](../../embeddings/search/embeddingindex-search.md).

Query transfer and result read-back are per call; the matrix is not. A scores buffer is allocated
per call and used as scratch — the selection masks each taken row to negative infinity, so the
buffer cannot be reused and is never handed back.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledCosineTopK`](tiledcosinetopk.md).
