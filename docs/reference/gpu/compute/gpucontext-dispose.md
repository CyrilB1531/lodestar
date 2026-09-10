# GpuContext.Dispose

Frees the accelerator and then the context, in that order.

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

**Remarks** — Every `Device*` type built against this context must be disposed **before** it: they hold buffers the accelerator owns, and releasing the accelerator first leaves them pointing at nothing.

`using` is the intended spelling. Device memory is not reclaimed by the garbage collector, so a
block that is dropped rather than disposed stays allocated on the accelerator until the process
ends — which on a card with a few gigabytes is a leak that a long-running service notices.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`GpuContext`](gpucontext.md).
