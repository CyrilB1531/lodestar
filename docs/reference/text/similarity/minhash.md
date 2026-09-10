# MinHash

MinHash signatures, and the Jaccard estimate two of them give.

<!-- docs-declaration -->

```csharp
public sealed class MinHash
```

**Example** — two sentences differing by one word.

```csharp
using Lodestar.Text.Similarity;

var permutations = new MinHashPermutations([3UL, 5UL, 7UL, 11UL], [13UL, 17UL, 19UL, 23UL]);
var hasher = new MinHash(permutations);

uint[] fox = hasher.Signature(["the", "quick", "brown", "fox"]);
uint[] dog = hasher.Signature(["the", "quick", "brown", "dog"]);

double estimate = MinHash.Jaccard(fox, dog);  // => 0.75
int length = hasher.Length;  // => 4
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`MinHash.Signature`](minhash-signature.md) | The signature of a set of tokens |
| [`MinHash.Jaccard`](minhash-jaccard.md) | The estimate two signatures give |

**Remarks** — reference behaviour is `datasketch` 1.6.5's `MinHash`. A signature is one minimum
per permutation, so **it sees a set and not a bag**: a token repeated changes nothing, because a
minimum is idempotent. That is the line between this and [`SimHash`](simhash.md), which sums
weights and does see the repeat.

**Four permutations is a documentation size, not a working one.** The estimate is a share of
agreeing slots, so its resolution is `1 / Length` — four permutations can only ever answer 0,
0.25, 0.5, 0.75 or 1. The reference defaults to 128, and an index that has to separate 0.78 from
0.82 needs at least that.

Exact against the reference, and exactly rather than within a tolerance: a signature is a list of
hashes, so any difference at all means the algorithm diverged rather than the arithmetic. The
corpus in `tests/oracles/text_similarity.json` freezes eight documents and the permutations they
were built with.

Immutable once built, and safe to use from any number of threads at once.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SimHash`](simhash.md), [`LshIndex`](lshindex.md),
[`MinHashPermutations`](minhashpermutations.md), [the set-similarity index](../similarity.md).
