# MinHashScheme

Which permutation family a [`MinHash`](minhash.md) signature is built from.

<!-- docs-declaration -->

```csharp
public enum MinHashScheme
```

**Example** — the two families over the same tokens, which agree on nothing.

```csharp
using Lodestar.Text.Similarity;

string[] tokens = ["the", "quick", "brown", "fox"];

ulong[] legacyA = [775169054918279404, 2109959069025162];
ulong[] legacyB = [1758426461858698312, 965365488286768773];
var legacy = new MinHash(new MinHashPermutations(legacyA, legacyB));

ulong[] affineA = [3582191691, 4270784983];
ulong[] affineB = [982527, 1100580627];
var affine = new MinHash(new MinHashPermutations(affineA, affineB, MinHashScheme.Affine32));

bool same = legacy.Signature(tokens).SequenceEqual(affine.Signature(tokens));  // => False
```

**Members** — two, and they share no value.

| Member | The arithmetic |
| --- | --- |
| `Legacy` | `(a·h + b) mod (2^61 − 1)`, masked to 32 bits |
| `Affine32` | `a·fmix32(h) + b` in 32-bit arithmetic, with an odd `a` |

**Remarks** — `datasketch` had one family through 1.6.5 and named three in 2.0.0, **making
`affine32` the default**. So the same reference call that returned `Legacy` values now returns
`Affine32` ones, and a caller comparing against a current `datasketch` needs the second family
rather than the first.

`Legacy` is the default here, so a signature built before 2.0.0 existed keeps comparing to one
built after. `datasketch` 2.0.0 still computes it, spelled `scheme="legacy"`, and the two agree
coefficient for coefficient.

**`Affine32` needs no prime modulus**, which is the point of it: an odd multiplier makes the map a
bijection, because a collision would need `2^32` to divide `a·(h₁ − h₂)` and an odd `a` contributes
no factor of two. The MurmurHash3 finalizer runs once per token first, so a weakly hashed input
cannot ride its own structure through an affine map — which is what the prime modulus used to
absorb.

Measured on this repository's corpus, the second family costs about half the first in the loop
that runs once per token per permutation, and hashing dominates both:
[decision 0108](../../../decisions/0108-minhash-ships-both-permutation-families.md) has the
numbers and why neither replaces the other.

**`affine64` is not offered.** It changes the hash function as well — the reference pairs it with
`sha1_hash64` rather than `sha1_hash32` — so it is a second parity surface and not a third value
of this one.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinHash`](minhash.md), [`MinHashPermutations`](minhashpermutations.md),
[the set-similarity index](../similarity.md).
