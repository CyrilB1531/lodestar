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
MinHashScheme read = permutations.Scheme;  // => Legacy
```

**Example** — the same shape under the reference's current default, which reads 32-bit
coefficients and needs an odd multiplier.

```csharp
using Lodestar.Text.Similarity;

ulong[] odd = [3, 5, 7, 11];
ulong[] addends = [13, 17, 19, 23];
var affine = new MinHashPermutations(odd, addends, MinHashScheme.Affine32);

MinHashScheme scheme = affine.Scheme;  // => Affine32
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

**The scheme travels with the coefficients**, because they are chosen together. An
[`MinHashScheme.Affine32`](minhashscheme.md) multiplier is odd and fits 32 bits where a
`Legacy` one spans 61, so the two families cannot share a set — and reading one family's
coefficients through the other's arithmetic produces a signature rather than an error, which
is why the constructor refuses the mismatch instead of computing it.

Naming no scheme means `Legacy`, so a signature built before `affine32` existed keeps comparing
to one built after.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinHash`](minhash.md), [`MinHashScheme`](minhashscheme.md),
[the set-similarity index](../similarity.md).
