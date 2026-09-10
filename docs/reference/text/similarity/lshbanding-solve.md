# LshBanding.Solve

The banding that minimizes the weighted error at a threshold.

<!-- docs-declaration -->

```csharp
public static LshBanding Solve(double threshold, int permutations, double falsePositiveWeight = 0.5, double falseNegativeWeight = 0.5)
```

**Parameters** — `threshold` is the similarity a caller wants found, strictly inside `(0, 1)`.
`permutations` is the signature length available, which `b × r` may not exceed.
`falsePositiveWeight` and `falseNegativeWeight` are how much each error costs, non-negative.

**Returns** — `LshBanding`, the `(b, r)` pair with the lowest weighted error.

**Exceptions** — `ArgumentOutOfRangeException` when an argument is outside its range.

**Example** — a higher threshold asks for deeper bands.

```csharp
using Lodestar.Text.Similarity;

LshBanding loose = LshBanding.Solve(0.5, 128);
LshBanding tight = LshBanding.Solve(0.9, 128);

int looseRows = loose.RowsPerBand;  // => 5
int tightRows = tight.RowsPerBand;  // => 25
```

**Remarks** — the two errors are the areas under the S-curve on the wrong side of the threshold,
as the published LSH analysis states them, integrated by the midpoint rule at a step of `0.001`.
Measured: that reproduces the reference's own choice on all fifteen threshold and length pairs the
corpus freezes, and because the answer is a pair of integers a small difference in the quadrature
does not move it.

**The weights are the argument this exposes and the reference does not.** Equal weights are the
default and treat a wasted verification as costing exactly what a missed pair costs — which is
almost never true. Raising `falseNegativeWeight` asks for more bands and finds more; raising
`falsePositiveWeight` asks for deeper bands and verifies less.

The search is over every `(b, r)` with `b × r ≤ permutations`, so it costs
`O(permutations · log permutations)` curve evaluations. At 128 that is milliseconds, once, at
index construction.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LshBanding.CollisionProbability`](lshbanding-collisionprobability.md).
