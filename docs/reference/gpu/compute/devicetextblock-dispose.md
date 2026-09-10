# DeviceTextBlock.Dispose

Frees the two device buffers.

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

**Remarks** — The symbols and their offsets go together: the offsets alone cannot cut a batch that is no longer there.

`using` is the intended spelling. Device memory is not reclaimed by the garbage collector, so a
block that is dropped rather than disposed stays allocated on the accelerator until the process
ends — which on a card with a few gigabytes is a leak that a long-running service notices.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceTextBlock`](devicetextblock.md).
