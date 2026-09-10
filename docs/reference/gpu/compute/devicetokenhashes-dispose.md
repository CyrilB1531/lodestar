# DeviceTokenHashes.Dispose

Frees the two device buffers.

<!-- docs-declaration -->

```csharp
public void Dispose()
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var resident = DeviceTokenHashes.Upload(context, [[1u, 2u]]);

int held = resident.Count;  // => 1
```

**Remarks** — the hashes and their offsets go together: the offsets alone cannot cut a batch that is
no longer there.

`using` is the intended spelling. Device memory is not reclaimed by the garbage collector, so a
block that is dropped rather than disposed stays allocated on the accelerator until the process
ends.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceTokenHashes`](devicetokenhashes.md).
