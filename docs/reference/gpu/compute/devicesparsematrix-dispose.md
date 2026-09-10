# DeviceSparseMatrix.Dispose

Frees the three device buffers.

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

**Remarks** — Row pointers, column indices and values are three separate allocations, and all three go together: a CSR matrix missing one of them describes nothing.

`using` is the intended spelling. Device memory is not reclaimed by the garbage collector, so a
block that is dropped rather than disposed stays allocated on the accelerator until the process
ends — which on a card with a few gigabytes is a leak that a long-running service notices.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceSparseMatrix`](devicesparsematrix.md).
