# MinHashPermutations.Multiplier

The `a` coefficient of one permutation.

<!-- docs-declaration -->

```csharp
public ulong Multiplier(int index)
```

**Parameters** — `index` is which permutation, from zero.

**Returns** — `ulong`, the multiplier supplied for it.

**Exceptions** — `ArgumentOutOfRangeException` when `index` is outside the set.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Text.Similarity;

var permutations = new MinHashPermutations([3UL, 5UL], [13UL, 17UL]);

ulong second = permutations.Multiplier(1);  // => 5
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinHashPermutations.Addend`](minhashpermutations-addend.md).
