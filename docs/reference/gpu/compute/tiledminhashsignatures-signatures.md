# TiledMinHashSignatures.Signatures

The signature of every document in a resident batch.

<!-- docs-declaration -->

```csharp
public IReadOnlyList<uint[]> Signatures(DeviceTokenHashes documents, ReadOnlySpan<ulong> multipliers, ReadOnlySpan<ulong> addends)
public IReadOnlyList<uint[]> Signatures(DeviceTokenHashes documents, ReadOnlySpan<ulong> multipliers, ReadOnlySpan<ulong> addends, MinHashScheme scheme)
```

**Parameters** — `documents` is the resident token hashes. `multipliers` holds the `a` coefficient
of each permutation and `addends` the `b`, one per multiplier. `scheme` is which arithmetic the
coefficients belong to; naming none means [`MinHashScheme.Legacy`](minhashscheme.md).

**Returns** — one signature per document, each as long as there are permutations.

**Exceptions** — `ArgumentNullException` when `documents` is null; `ArgumentException` when the two
coefficient spans are not the same non-zero length, when `scheme` is not a declared member, or when
a coefficient does not fit the scheme it is given.

**Example** — an empty document gives every slot its maximum.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var resident = DeviceTokenHashes.Upload(context, [[]]);

IReadOnlyList<uint[]> signatures =
    new TiledMinHashSignatures(context).Signatures(resident, [3UL], [13UL]);

bool atMaximum = signatures[0][0] == uint.MaxValue;  // => True
```

**Example** — the same residency, read through the reference's current default.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var resident = DeviceTokenHashes.Upload(context, [[11u, 22u, 33u]]);
var kernel = new TiledMinHashSignatures(context);

IReadOnlyList<uint[]> legacy = kernel.Signatures(resident, [3UL], [13UL]);
IReadOnlyList<uint[]> affine =
    kernel.Signatures(resident, [3UL], [13UL], MinHashScheme.Affine32);

bool agree = legacy[0][0] == affine[0][0];  // => False
```

**Remarks** — that maximum is the **identity a minimum starts from**, not a sentinel, so two empty
documents estimate a similarity of 1 — exactly as they do on the CPU path. A caller who wants an
empty document to match nothing has to check for it before comparing.

The coefficients are uploaded per call and the batch is not, which is the same division every
kernel here makes: the small operand travels, the corpus stays. With 128 permutations that upload is
two kilobytes against a corpus of any size.

Signatures come back as one array per document rather than one flat block, because what a caller
does next is compare two of them — and `MinHash.Jaccard` takes two spans.

**One residency serves both families.** `Affine32` needs the MurmurHash3 finalizer applied to each
hash, and that runs **inside the kernel**, as the shared tile fills — once per token per group
rather than once per token per thread. Mixing before upload would have been cheaper still and would
have made a resident batch belong to one scheme, which is what residency exists not to do.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledMinHashSignatures`](tiledminhashsignatures.md),
[`MinHashScheme`](minhashscheme.md).
