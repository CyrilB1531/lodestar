# DeviceDenseBlock.Dispose

Frees the device memory the block holds.

<!-- docs-declaration -->

```csharp
public void Dispose()
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);

bool named = context.DeviceName.Length > 0;  // => True
```

**Remarks** — In a chain each intermediate is a block, so each is disposed as the chain moves past it. A `using` per step is the shape the samples use, and it is why a chain of three operations holds at most two intermediates at once.

`using` is the intended spelling. Device memory is not reclaimed by the garbage collector, so a
block that is dropped rather than disposed stays allocated on the accelerator until the process
ends — which on a card with a few gigabytes is a leak that a long-running service notices.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceDenseBlock`](devicedenseblock.md).
