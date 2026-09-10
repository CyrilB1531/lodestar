# MinHash.Jaccard

The estimated Jaccard similarity of two signatures.

<!-- docs-declaration -->

```csharp
public static double Jaccard(ReadOnlySpan<uint> left, ReadOnlySpan<uint> right)
```

**Parameters** — `left` and `right` are two signatures of the same non-zero length, built from
the same permutations.

**Returns** — `double`, the share of slots that agree, in `[0, 1]`.

**Exceptions** — `ArgumentException` when the two are not the same non-zero length.

**Example** — disjoint sets estimate zero.

```csharp
using Lodestar.Text.Similarity;

var hasher = new MinHash(new MinHashPermutations([3UL, 5UL, 7UL, 11UL], [13UL, 17UL, 19UL, 23UL]));

uint[] left = hasher.Signature(["alpha", "beta"]);
uint[] right = hasher.Signature(["gamma", "delta"]);

double estimate = MinHash.Jaccard(left, right);  // => 0
```

**Remarks** — an estimate, not the Jaccard index. It is unbiased, and its standard error is
about `1 / sqrt(Length)` — 128 permutations put it near 0.09, which is why a threshold and a
signature length are chosen together rather than separately.

**Signatures built from different permutations are not comparable**, and nothing here can detect
that: the lengths match, the values are hashes, and the answer comes back looking ordinary. One
`MinHashPermutations` per index is the discipline that avoids it.

The exact Jaccard index over the sets themselves is [`Jaccard.Similarity`](jaccard-similarity.md),
which is what to use when the sets are small enough to compare directly.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinHash.Signature`](minhash-signature.md), [`Jaccard`](jaccard.md).
