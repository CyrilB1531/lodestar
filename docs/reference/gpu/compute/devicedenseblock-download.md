# DeviceDenseBlock.Download

Copies the block back to the host, ending the chain.

<!-- docs-declaration -->

```csharp
public double[] Download()
```

**Returns** — `RowCount` × `ColumnCount` values, row-major.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var block = DeviceDenseBlock.Upload(context, [5.0, 6.0], rowCount: 1, columnCount: 2);

double[] values = block.Download();

double second = values[1];  // => 6
```

**Remarks** — the one device-to-host copy a chain pays, however many operations it held. **Calling
this between two kernels is what residency exists to avoid**, and the benchmark prices exactly
that: `RoundTripped` is the baseline against which `Chained` is measured.

**A download is a copy, not a move.** Calling it mid-chain to inspect an intermediate leaves the
block usable, and calling it twice gives the same values — which the tests pin, because a consuming
read would make the next step of the chain read whatever the buffer became.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceDenseBlock.Upload`](devicedenseblock-upload.md).
