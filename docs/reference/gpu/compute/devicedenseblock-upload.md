# DeviceDenseBlock.Upload

Uploads a host block to the accelerator.

<!-- docs-declaration -->

```csharp
public static DeviceDenseBlock Upload(GpuContext context, ReadOnlySpan<double> values, int rowCount, int columnCount)
```

**Parameters** — `context` is the accelerator to upload to. `values` holds `rowCount` ×
`columnCount` numbers, row-major. `rowCount` and `columnCount` are the block's shape, and the
product of the two has to be exactly `values.Length`.

**Returns** — `DeviceDenseBlock`, owning device memory the caller disposes.

**Exceptions** — `ArgumentNullException` when `context` is null; `ArgumentOutOfRangeException` when
a dimension is below 1; `ArgumentException` when `values` is not exactly the block.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var block = DeviceDenseBlock.Upload(context, [1.0, 2.0], rowCount: 1, columnCount: 2);

int wide = block.ColumnCount;  // => 2
```

**Remarks** — this is the **first** crossing of the bus in a chain, and the only one a caller
writes by hand. Everything after it comes from a kernel that leaves its result resident, which is
the overload on [`TiledSparseDenseProduct.Multiply`](tiledsparsedenseproduct-multiply.md) that
takes a block and returns one.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceDenseBlock.Download`](devicedenseblock-download.md).
