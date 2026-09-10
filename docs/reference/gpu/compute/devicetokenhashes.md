# DeviceTokenHashes

One document's token hashes per row, held flat on the accelerator.

<!-- docs-declaration -->

```csharp
public sealed class DeviceTokenHashes : IDisposable
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
uint[][] documents = [[11u, 22u, 33u], [22u, 44u]];
using var resident = DeviceTokenHashes.Upload(context, documents);

int held = resident.Count;  // => 2
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`DeviceTokenHashes.Upload`](devicetokenhashes-upload.md) | Flattens a batch of documents and uploads it |
| [`DeviceTokenHashes.Dispose`](devicetokenhashes-dispose.md) | Frees the two device buffers |

**Properties** — `Count` is how many documents the block holds.

**Remarks** — **hashes rather than tokens**, for the reason every other type here takes data rather
than a Lodestar type: a kernel parameter must be blittable, and taking the hashes keeps this package
free of an edge in either direction.

That is also the split that makes the kernel worth running. Hashing a token is cheap and
sequential, so the host keeps it; minimising over the permutations is `O(tokens × permutations)`,
which is the part an accelerator is for.

**The hash has to be the one the CPU path uses, or the signatures will not match.**
`Lodestar.Text.Similarity.MinHash` hashes a token as the first four bytes of its SHA-1,
little-endian — the construction `datasketch` exports as `sha1_hash32`. Supply those and the two
agree exactly; supply anything else and you have a different sketch, which is a legitimate thing to
want and is not parity.

A document may be empty, and repeats are harmless: a minimum is idempotent, which is what makes
this a **set** sketch rather than a bag one.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledMinHashSignatures`](tiledminhashsignatures.md),
[the namespace index](../compute.md).
