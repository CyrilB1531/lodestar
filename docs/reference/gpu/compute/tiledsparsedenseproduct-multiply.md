# TiledSparseDenseProduct.Multiply

Computes `matrix · block`, row-major.

<!-- docs-declaration -->

```csharp
public double[] Multiply(DeviceSparseMatrix matrix, ReadOnlySpan<double> block, int width)
public DeviceDenseBlock Multiply(DeviceSparseMatrix matrix, DeviceDenseBlock block)
```

**Parameters** — `matrix` is the resident sparse left operand, on both overloads. `block` is the
dense right operand: on the first overload a host span of `matrix.ColumnCount` rows by `width` — the
same shape [`CsrMatrix.Multiply`](../../abstractions/sparse/csrmatrix-multiply.md) takes — and on the second a **resident** block, which is the entry
point that chains. `width` says how many columns the host span holds, and is read from the resident
block instead on the second overload.

**Returns** — `double[]` of `matrix.RowCount` × `width` for the host overload;
`DeviceDenseBlock` of `matrix.RowCount` × `block.ColumnCount` for the resident one.

**Exceptions** — `ArgumentNullException` when an argument is null;
`ArgumentOutOfRangeException` when `width` is below 1; `ArgumentException` when the operands do not
compose.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var matrix = DeviceSparseMatrix.Upload(
    context, [0, 1], [0], [4.0], rowCount: 1, columnCount: 2);
var kernel = new TiledSparseDenseProduct(context);

using var operand = DeviceDenseBlock.Upload(context, [1.0, 2.0, 3.0, 4.0], 2, 2);
using DeviceDenseBlock resident = kernel.Multiply(matrix, operand);

double first = resident.Download()[0];  // => 4
```

**Remarks** — **the host overload delegates to the resident one**, so the convenience entry point
and the chaining entry point cannot compute different answers. That the suite's parity tests pass
through both unchanged is the evidence for it.

The resident overload is what makes a chain: its result is the type its own operand is, so a second
product consumes it without crossing the bus. The caller ends the chain with
[`DeviceDenseBlock.Download`](devicedenseblock-download.md) and pays one copy for however many steps
it held — measured at **1.24× to 2.94×** faster than the same steps round-tripped.

**The host overload is not a chain of length one.** It uploads, multiplies and downloads, so using
it twice in a row pays four crossings where the resident overload pays two.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledSparseDenseProduct`](tiledsparsedenseproduct.md).
