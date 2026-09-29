# MinHashScheme

Which permutation family a signature is built from.

<!-- docs-declaration -->

```csharp
public enum MinHashScheme
```

**Example** — one resident batch, both families.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var resident = DeviceTokenHashes.Upload(context, [[11u, 22u, 33u]]);
var kernel = new TiledMinHashSignatures(context);

IReadOnlyList<uint[]> legacy =
    kernel.Signatures(resident, [3UL], [13UL], MinHashScheme.Legacy);
IReadOnlyList<uint[]> affine =
    kernel.Signatures(resident, [3UL], [13UL], MinHashScheme.Affine32);

bool agree = legacy[0][0] == affine[0][0];  // => False
```

**Members** — two, and they share no value.

| Member | The arithmetic |
| --- | --- |
| `Legacy` | `(a·h + b) mod (2^61 − 1)`, masked to 32 bits |
| `Affine32` | `a·fmix32(h) + b` in 32-bit arithmetic, with an odd `a` |

**Remarks** — the same two families `Lodestar.Text.Similarity.MinHashScheme` names, and **a
separate enum**. Both live in `Lodestar.Abstractions` since
[#1142](https://github.com/CyrilB1531/lodestar/issues/1142), each under the namespace it first
shipped in and forwarded from its package, so code built against either keeps binding; merging them
would break one set of callers. This package takes no edge to `Lodestar.Text`, which is why
`MersennePrime` and the 32-bit mask are spelled twice.

The cost is visible to a caller using both: `using Lodestar.Gpu.Compute;` and
`using Lodestar.Text.Similarity;` together make the bare name ambiguous, and one of them needs a
`using` alias.

`Affine32` applies the MurmurHash3 finalizer **inside the kernel**, as the shared tile fills, so
one [`DeviceTokenHashes`](devicetokenhashes.md) serves both families rather than belonging to one.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledMinHashSignatures.Signatures`](tiledminhashsignatures-signatures.md),
[the namespace index](../compute.md).
