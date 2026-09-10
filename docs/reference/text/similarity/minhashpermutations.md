# MinHashPermutations

The permutation coefficients a [`MinHash`](minhash.md) signature is built from.

<!-- docs-declaration -->

```csharp
public sealed class MinHashPermutations
```

**Example** — four permutations, supplied rather than seeded.

```csharp
using Lodestar.Text.Similarity;

ulong[] multipliers = [3, 5, 7, 11];
ulong[] addends = [13, 17, 19, 23];
var permutations = new MinHashPermutations(multipliers, addends);

int count = permutations.Count;  // => 4
ulong first = permutations.Multiplier(0);  // => 3
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`MinHashPermutations.Multiplier`](minhashpermutations-multiplier.md) | The `a` coefficient of one permutation |
| [`MinHashPermutations.Addend`](minhashpermutations-addend.md) | The `b` coefficient of one permutation |

**Remarks** — **the permutations are an input, not a seed.** That is the same call
[decision 0072](../../../decisions/0072-omega-is-an-input-not-a-seed.md) made for randomized
SVD's Ω, and for the same reason: a randomized algorithm whose randomness is supplied is an
ordinary parity target, where one deriving it from a seed would have to reproduce another
library's generator stream to agree with it.

`datasketch` exposes its own pair as `MinHash.permutations`, so a caller comparing against it
passes those; a caller who only needs *some* permutations can pass any values, and two indexes
compared against each other must use the same ones.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinHash`](minhash.md), [the set-similarity index](../similarity.md).
