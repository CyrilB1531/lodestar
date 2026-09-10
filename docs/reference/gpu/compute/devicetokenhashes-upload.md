# DeviceTokenHashes.Upload

Flattens a batch of documents and uploads it.

<!-- docs-declaration -->

```csharp
public static DeviceTokenHashes Upload(GpuContext context, IReadOnlyList<IReadOnlyList<uint>> documents)
```

**Parameters** — `context` is the accelerator to upload to. `documents` holds one sequence of token
hashes per document; a document may be empty, and repeats are harmless.

**Returns** — `DeviceTokenHashes`, owning two device buffers the caller disposes.

**Exceptions** — `ArgumentNullException` when `context`, `documents`, or one of the documents, is
null; `ArgumentException` when `documents` is empty.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var resident = DeviceTokenHashes.Upload(context, [[7u, 9u], []]);

int held = resident.Count;  // => 2
```

**Remarks** — the batch is flattened into one array with an offset per document, the same shape
[`DeviceTextBlock`](devicetextblock.md) uses: a jagged array cannot cross into a kernel, and one
allocation beats one per document.

**An empty *document* is legal; an empty *batch* is not.** The first is a set with no members and
gets a signature of every slot at its maximum; the second leaves nothing to launch a kernel over.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceTokenHashes`](devicetokenhashes.md).
