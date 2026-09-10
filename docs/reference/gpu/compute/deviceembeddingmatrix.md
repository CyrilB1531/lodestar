# DeviceEmbeddingMatrix

A row-major embedding matrix held on the accelerator across many queries.

<!-- docs-declaration -->

```csharp
public sealed class DeviceEmbeddingMatrix : IDisposable
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
float[] rows = [1f, 0f, 0f, 1f, 1f, 0f];
using var matrix = DeviceEmbeddingMatrix.Upload(context, rows, count: 3, dimension: 2);

int held = matrix.Count;  // => 3
int wide = matrix.Dimension;  // => 2
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`DeviceEmbeddingMatrix.Upload`](deviceembeddingmatrix-upload.md) | Uploads a row-major block of vectors |
| [`DeviceEmbeddingMatrix.Dispose`](deviceembeddingmatrix-dispose.md) | Frees the device memory the matrix holds |

**Properties** — `Count` is how many rows, `Dimension` how many values each holds.

**Remarks** — **residency is the point.** A GPU package that uploads its corpus per call is slower
than the SIMD path it replaces: measured at **9.25× slower** for a hundred thousand documents
answering one query, against 6.6× *faster* once the same matrix is resident. The crossing point is
around a hundred queries per corpus.

This is residency only. [Decision 0102](../../../decisions/0102-the-gpu-gate-is-measured-on-a-named-machine.md)
defers the chainable device-resident types until three kernels exist, because chainability is a
claim about two operations sharing a residency — [`DeviceDenseBlock`](devicedenseblock.md) is that
half, and it carries doubles rather than the floats an embedding matrix holds.

Rows are L2-normalized on upload by default, which is what turns cosine similarity into a dot
product. A zero row is left alone rather than divided by zero.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledCosineTopK`](tiledcosinetopk.md), [the namespace index](../compute.md).
