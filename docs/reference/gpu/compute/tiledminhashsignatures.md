# TiledMinHashSignatures

MinHash signatures for a batch of documents, one thread per permutation.

<!-- docs-declaration -->

```csharp
public sealed class TiledMinHashSignatures
```

**Example** — the shape a caller writes.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var resident = DeviceTokenHashes.Upload(context, [[11u, 22u], [22u, 33u]]);
var kernel = new TiledMinHashSignatures(context);

ulong[] multipliers = [3UL, 5UL];
ulong[] addends = [13UL, 17UL];
IReadOnlyList<uint[]> signatures = kernel.Signatures(resident, multipliers, addends);

int documents = signatures.Count;  // => 2
int length = signatures[0].Length;  // => 2
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`TiledMinHashSignatures.Signatures`](tiledminhashsignatures-signatures.md) | The signature of every document in a batch |

**Remarks** — **the work splits where the parallelism is.** Hashing a token is cheap and
sequential, so the host keeps it and hands over [`DeviceTokenHashes`](devicetokenhashes.md);
minimising over the permutations is `O(tokens × permutations)` and is what the accelerator takes. A
thread owns one permutation of one document and reduces in a register, while the document's hashes
are tiled through shared memory so a group reads them once rather than once per thread.

The coefficients are **supplied rather than seeded**, which is the call
`Lodestar.Text.Similarity.MinHashPermutations` already makes for the same reason — deriving them
from a seed would make parity depend on a random number generator instead of on an algorithm. Taking
them as spans is also what keeps this package free of an edge.

**The permuted value is masked with an AND, not a modulo.** The two agree only below the mask, so
the choice moves every signature rather than rounding one, and the CPU path records the measured
pair of values that proves it.

**Build this once.** Loading compiles, and a cold launch measures ILGPU's compiler.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`DeviceTokenHashes`](devicetokenhashes.md), [the namespace index](../compute.md).
