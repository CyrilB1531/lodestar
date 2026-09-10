# MinHashPermutations.Addend

The `b` coefficient of one permutation.

<!-- docs-declaration -->

```csharp
public ulong Addend(int index)
```

**Parameters** — `index` is which permutation, from zero.

**Returns** — `ulong`, the addend supplied for it.

**Exceptions** — `ArgumentOutOfRangeException` when `index` is outside the set.

**Example** — the shape a caller writes.

```csharp
using Lodestar.Text.Similarity;

var permutations = new MinHashPermutations([3UL, 5UL], [13UL, 17UL]);

ulong second = permutations.Addend(1);  // => 17
```

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MinHashPermutations.Multiplier`](minhashpermutations-multiplier.md).
