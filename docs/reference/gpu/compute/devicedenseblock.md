# DeviceDenseBlock

A row-major dense block held on the accelerator between two operations.

<!-- docs-declaration -->

```csharp
public sealed class DeviceDenseBlock : IDisposable
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var block = DeviceDenseBlock.Upload(
    context, [1.0, 2.0, 3.0, 4.0], rowCount: 2, columnCount: 2);

int rows = block.RowCount;  // => 2
double first = block.Download()[0];  // => 1
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`DeviceDenseBlock.Upload`](devicedenseblock-upload.md) | Uploads a host block to the accelerator |
| [`DeviceDenseBlock.Download`](devicedenseblock-download.md) | Copies the block back, ending the chain |
| [`DeviceDenseBlock.Dispose`](devicedenseblock-dispose.md) | Frees the device memory the block holds |

**Properties** — `RowCount` and `ColumnCount`.

**Remarks** — **this is the type that makes a chain a chain.** A kernel returning `double[]` has
already paid a device-to-host copy, so the next one pays a host-to-device copy undoing it. A block
produced on the accelerator and consumed there crosses the bus once at each end of the chain
instead, and `Download` is where a caller says the chain is over.

Measured: two sparse-dense products chained ran **1.24× to 2.94×** faster than the same two with a
download and re-upload between them. The gain is largest where the work is *smallest*, which
inverts the usual intuition — a large job amortises a round trip on its own.

[Decision 0102](../../../decisions/0102-the-gpu-gate-is-measured-on-a-named-machine.md) deferred
this type until three kernels existed, on the ground that chainability is a claim about two
operations sharing a residency and cannot be measured with one.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledSparseDenseProduct`](tiledsparsedenseproduct.md),
[the namespace index](../compute.md).
