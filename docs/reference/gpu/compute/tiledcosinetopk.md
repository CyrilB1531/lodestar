# TiledCosineTopK

Sweeps a resident matrix with a batch of queries: cosine similarity, then top-k.

<!-- docs-declaration -->

```csharp
public sealed class TiledCosineTopK
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
float[] rows = [1f, 0f, 0f, 1f, 1f, 1f];
using var matrix = DeviceEmbeddingMatrix.Upload(context, rows, count: 3, dimension: 2);
var kernel = new TiledCosineTopK(context);

IReadOnlyList<IReadOnlyList<GpuSearchResult>> hits = kernel.Search(matrix, [1f, 0f], 1, 2);

int best = hits[0][0].Index;  // => 0
int howMany = hits[0].Count;  // => 2
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`TiledCosineTopK.Search`](tiledcosinetopk-search.md) | The best *k* rows for each query in a batch |

**Remarks** — two kernels on one stream. The first tiles the **query** vector through shared
memory so a group reads it once rather than once per thread; the second selects the best *k* by
repeated parallel argmax, keeping the selection on the accelerator instead of copying every score
back.

**Ties break by row index ascending**, matching [`EmbeddingIndex.Search`](../../embeddings/search/embeddingindex-search.md). That is the one thing the
CPU sort and the GPU reduction have to agree on by construction rather than by luck, and a corpus
of identical rows is what the tests use to prove it.

**Build this once.** Loading compiles, and a first launch on a freshly loaded kernel measures
ILGPU's compiler — which is why
[decision 0102](../../../decisions/0102-the-gpu-gate-is-measured-on-a-named-machine.md) asks a
benchmark for an explicit warm-up.

The group size is clamped to what the accelerator allows and rounded down to a power of two:
ILGPU's CPU accelerator caps a group dimension at **16**, not 256, so a kernel hard-coded to 256
threads runs on a GPU and refuses to launch exactly where correctness is tested.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceEmbeddingMatrix`](deviceembeddingmatrix.md),
[`GpuSearchResult`](gpusearchresult.md), [the namespace index](../compute.md).
