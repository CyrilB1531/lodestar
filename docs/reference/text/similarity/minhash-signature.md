# MinHash.Signature

The signature of a set of tokens.

<!-- docs-declaration -->

```csharp
public uint[] Signature(IEnumerable<string> tokens)
```

**Parameters** — `tokens` is the set. Repeats change nothing, because a minimum is idempotent,
which is what makes this a set sketch rather than a bag one.

**Returns** — `uint[]`, one value per permutation.

**Exceptions** — `ArgumentNullException` when `tokens`, or a token in it, is null.

**Example** — the same set written two ways.

```csharp
using Lodestar.Text.Similarity;

var hasher = new MinHash(new MinHashPermutations([3UL, 5UL], [13UL, 17UL]));

uint[] ordered = hasher.Signature(["alpha", "beta"]);
uint[] reversed = hasher.Signature(["beta", "alpha"]);

bool same = ordered.SequenceEqual(reversed);  // => True
```

**Remarks** — an **empty set gives every slot `uint.MaxValue`**, which is the identity a minimum
starts from rather than a sentinel. Two empty sets therefore estimate a similarity of 1, and a
caller who wants an empty set to match nothing has to check for it before comparing.

Each token is hashed to 32 bits by the first four bytes of its SHA-1, little-endian — the
reference exports that as `sha1_hash32`, so it is part of the contract rather than an internal
choice. SHA-1 is a hash function here and never a signature.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinHash.Jaccard`](minhash-jaccard.md).
