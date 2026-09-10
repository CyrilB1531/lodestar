# GpuSearchResult

One hit from a device sweep: the row's index and its cosine similarity.

<!-- docs-declaration -->

```csharp
public readonly record struct GpuSearchResult(int Index, float Score)
```

**Parameters** — `Index` is the row of the device matrix that matched. `Score` is the cosine
similarity, higher is better.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

var hit = new GpuSearchResult(Index: 7, Score: 0.5f);

int row = hit.Index;  // => 7
```

**Remarks** — deliberately the **same shape** as `Lodestar.Embeddings.Search.SearchResult` without
being it. [Decision 0101](../../../decisions/0101-lodestar-gpu-is-the-one-package-that-does-not-ship-netstandard2-0.md)
forbids an edge from a core package into this one, and an edge the other way would floor this
package on a published sibling for the sake of one struct.

`Index` is an **index, not an identifier**: it means a row of the matrix that was uploaded, and
mapping it back to a document of your own is the caller's job — the same contract
`DeviceEmbeddingMatrix` has with the block it was given.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledCosineTopK`](tiledcosinetopk.md), [the namespace index](../compute.md).
