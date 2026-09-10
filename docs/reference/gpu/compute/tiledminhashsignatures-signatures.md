# TiledMinHashSignatures.Signatures

The signature of every document in a resident batch.

<!-- docs-declaration -->

```csharp
public IReadOnlyList<uint[]> Signatures(DeviceTokenHashes documents, ReadOnlySpan<ulong> multipliers, ReadOnlySpan<ulong> addends)
```

**Parameters** — `documents` is the resident token hashes. `multipliers` holds the `a` coefficient
of each permutation and `addends` the `b`, one per multiplier.

**Returns** — one signature per document, each as long as there are permutations.

**Exceptions** — `ArgumentNullException` when `documents` is null; `ArgumentException` when the two
coefficient spans are not the same non-zero length.

**Example** — an empty document gives every slot its maximum.

```csharp
using Lodestar.Gpu.Compute;

using var context = GpuContext.Create(preferCpu: true);
using var resident = DeviceTokenHashes.Upload(context, [[]]);

IReadOnlyList<uint[]> signatures =
    new TiledMinHashSignatures(context).Signatures(resident, [3UL], [13UL]);

bool atMaximum = signatures[0][0] == uint.MaxValue;  // => True
```

**Remarks** — that maximum is the **identity a minimum starts from**, not a sentinel, so two empty
documents estimate a similarity of 1 — exactly as they do on the CPU path. A caller who wants an
empty document to match nothing has to check for it before comparing.

The coefficients are uploaded per call and the batch is not, which is the same division every
kernel here makes: the small operand travels, the corpus stays. With 128 permutations that upload is
two kilobytes against a corpus of any size.

Signatures come back as one array per document rather than one flat block, because what a caller
does next is compare two of them — and `MinHash.Jaccard` takes two spans.

**Applies to** — net10.0, netstandard2.1.

**See also** — [`TiledMinHashSignatures`](tiledminhashsignatures.md).
