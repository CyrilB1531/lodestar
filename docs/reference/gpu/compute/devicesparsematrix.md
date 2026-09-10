# DeviceSparseMatrix

A CSR matrix held on the accelerator across many products.

<!-- docs-declaration -->

```csharp
public sealed class DeviceSparseMatrix : IDisposable
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var matrix = DeviceSparseMatrix.Upload(
    context, [0, 1, 2], [0, 1], [2.0, 3.0], rowCount: 2, columnCount: 2);

int rows = matrix.RowCount;  // => 2
int stored = matrix.NonZeroCount;  // => 2
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`DeviceSparseMatrix.Upload`](devicesparsematrix-upload.md) | Uploads a CSR matrix to the accelerator |
| [`DeviceSparseMatrix.Dispose`](devicesparsematrix-dispose.md) | Frees the three device buffers |

**Properties** — `RowCount`, `ColumnCount` and `NonZeroCount`.

**Remarks** — the three CSR arrays are taken as **spans rather than as a `CsrMatrix`**.
[Decision 0101](../../../decisions/0101-lodestar-gpu-is-the-one-package-that-does-not-ship-netstandard2-0.md)
forbids an edge from a core package into this one, and an edge the other way would floor this
package on a published `Lodestar.Abstractions` for the sake of one type. A caller holding a
`CsrMatrix` passes its `RowPointers`, `ColumnIndices` and `Values` directly.

Double precision, because `CsrMatrix` is. A consumer card runs FP64 at a fraction of FP32, and
the kernel clears its gate anyway — it is bound by memory bandwidth rather than by the
double-precision units, which is the one prediction this package got wrong in the kernel's favour.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledSparseDenseProduct`](tiledsparsedenseproduct.md),
[the namespace index](../compute.md).
