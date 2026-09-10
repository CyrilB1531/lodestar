# DeviceEmbeddingMatrix.Dispose

Frees the device memory the matrix holds.

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

**Remarks** — A matrix outlives many queries by design, so this is called once per corpus rather than once per search — which is the whole reason the type exists.

`using` is the intended spelling. Device memory is not reclaimed by the garbage collector, so a
block that is dropped rather than disposed stays allocated on the accelerator until the process
ends — which on a card with a few gigabytes is a leak that a long-running service notices.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceEmbeddingMatrix`](deviceembeddingmatrix.md).
