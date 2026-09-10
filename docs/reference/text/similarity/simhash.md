# SimHash

Charikar's SimHash: one 64-bit fingerprint per document, near-duplicates near each other.

<!-- docs-declaration -->

```csharp
public static class SimHash
```

**Example** — two sentences differing by one word.

```csharp
using Lodestar.Text.Similarity;

ulong fox = SimHash.Fingerprint(["the", "quick", "brown", "fox"]);
ulong dog = SimHash.Fingerprint(["the", "quick", "brown", "dog"]);

int apart = SimHash.HammingDistance(fox, dog);  // => 12
int bits = SimHash.Bits;  // => 64
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`SimHash.Fingerprint`](simhash-fingerprint.md) | The fingerprint of a bag of tokens |
| [`SimHash.HammingDistance`](simhash-hammingdistance.md) | How many bits two fingerprints differ in |

**Remarks** — reference behaviour is `simhash` 2.1.2. Where [`MinHash`](minhash.md) estimates
Jaccard over a **set**, this estimates cosine over a **weighted bag** — so a token repeated twice
counts twice. That difference is the whole basis for choosing between them, and it is the one a
frozen corpus pins rather than leaves to a reader.

**A fingerprint is one number, and that is both the appeal and the limit.** Storing it costs eight
bytes where a MinHash signature costs four per permutation, and comparing two costs one XOR and a
population count. What it cannot do is give a similarity: the Hamming distance is a rank, not a
ratio, so a caller wanting "how alike, on a scale" wants MinHash.

Thread-safe: every member is static and holds nothing.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinHash`](minhash.md), [the set-similarity index](../similarity.md).
