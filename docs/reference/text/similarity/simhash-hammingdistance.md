# SimHash.HammingDistance

How many bits two fingerprints differ in.

<!-- docs-declaration -->

```csharp
public static int HammingDistance(ulong left, ulong right)
```

**Parameters** — `left` and `right` are two fingerprints.

**Returns** — `int`, a count in `[0, 64]`. Zero means the two documents fingerprint alike.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Text.Similarity;

ulong same = SimHash.Fingerprint(["alpha", "beta"]);

int apart = SimHash.HammingDistance(same, same);  // => 0
```

**Remarks** — the usual near-duplicate threshold is **three bits or fewer out of 64**, which is
the figure the published work uses for web pages. It is not a universal constant: it depends on
how many tokens a document holds, and a short document's fingerprint moves further per token than
a long one's.

**Zero means the fingerprints agree, not that the documents do.** Sixty-four bits over a large
corpus will collide, which is why a fingerprint match is a candidate to verify rather than an
answer — the same relationship [`LshIndex`](lshindex.md) has with
[`MinHash.Jaccard`](minhash-jaccard.md).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SimHash.Fingerprint`](simhash-fingerprint.md).
