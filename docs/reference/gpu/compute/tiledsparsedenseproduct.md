# TiledSparseDenseProduct

The product of a resident CSR matrix with a dense block, tiled through shared memory.

<!-- docs-declaration -->

```csharp
public sealed class TiledSparseDenseProduct
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var matrix = DeviceSparseMatrix.Upload(
    context, [0, 1, 2], [0, 1], [2.0, 3.0], rowCount: 2, columnCount: 2);
var kernel = new TiledSparseDenseProduct(context);

double[] product = kernel.Multiply(matrix, [1.0, 0.0, 0.0, 1.0], width: 2);

double first = product[0];  // => 2
double last = product[3];  // => 3
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`TiledSparseDenseProduct.Multiply`](tiledsparsedenseproduct-multiply.md) | Computes `matrix · block` |

**Remarks** — one group per row and column tile. The group loads a tile of the row's stored values
**and their column indices** into shared memory, so a row's non-zeros are read from global memory
once per group rather than once per thread.

**Accumulation walks the row in stored order — the order [`CsrMatrix.Multiply`](../../abstractions/sparse/csrmatrix-multiply.md) walks it — so the
two agree far inside the tolerance the tests assert.** A looser agreement would mean the orders had
diverged, which is why the tests compare at `1e-6` and the real agreement is much tighter.

Double precision, because the CPU operand is. `bench/README.md` predicted that would cost the gate
on a consumer card; it does not, because the kernel is bound by memory bandwidth rather than by the
double-precision units. It clears at **8.66×** and **18.9×** at vectorizer sizes and misses below
them, which is where the crossing point belongs.

**Build this once.** Loading compiles, and a cold launch measures ILGPU's compiler.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceSparseMatrix`](devicesparsematrix.md),
[`DeviceDenseBlock`](devicedenseblock.md), [the namespace index](../compute.md).
