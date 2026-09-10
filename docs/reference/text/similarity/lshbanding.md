# LshBanding

How a MinHash signature is cut into bands for locality-sensitive hashing.

<!-- docs-declaration -->

```csharp
public readonly record struct LshBanding(int Bands, int RowsPerBand)
```

**Parameters** — `Bands` is `b`, how many bands the signature is cut into. `RowsPerBand` is `r`,
how many signature slots each band holds.

**Example** — the banding a threshold of 0.8 asks for over 128 permutations.

```csharp
using Lodestar.Text.Similarity;

LshBanding banding = LshBanding.Solve(0.8, 128);

int bands = banding.Bands;  // => 9
int rows = banding.RowsPerBand;  // => 13
int used = banding.Permutations;  // => 117
```

**Members** — one page each.

| Member | What it does |
| --- | --- |
| [`LshBanding.Solve`](lshbanding-solve.md) | The banding minimizing the weighted error at a threshold |
| [`LshBanding.CollisionProbability`](lshbanding-collisionprobability.md) | The S-curve this banding produces |

**Remarks** — **`b` and `r` are inputs, and the solve that picks them is a function you can
call.** The reference hides the same solve inside a constructor, which is what makes the trade-off
hard to see: a threshold does not select a banding on its own — it selects one *given how much a
caller minds a false positive against a false negative*.

**The solve rarely uses every slot.** Nine bands of thirteen is 117 of the 128 available, because
`b × r` has to divide into the length and the best pair usually leaves a remainder.
[`LshIndex`](lshindex.md) therefore accepts a signature longer than the banding needs and ignores
the tail rather than refusing it.

More bands finds more and verifies more; more rows per band finds less and wastes less. The
S-curve `1 - (1 - s^r)^b` is the whole story, and
[`CollisionProbability`](lshbanding-collisionprobability.md) draws it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LshIndex`](lshindex.md), [`MinHash`](minhash.md),
[the set-similarity index](../similarity.md).
