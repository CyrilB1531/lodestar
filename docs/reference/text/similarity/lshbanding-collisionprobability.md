# LshBanding.CollisionProbability

The chance two sets of a given similarity land in the same band.

<!-- docs-declaration -->

```csharp
public double CollisionProbability(double jaccard)
```

**Parameters** — `jaccard` is their Jaccard similarity, in `[0, 1]`.

**Returns** — `double`, the value `1 - (1 - s^r)^b` — the S-curve this banding produces.

**Exceptions** — `ArgumentOutOfRangeException` when `jaccard` is outside `[0, 1]`.

**Example** — the curve is steep around the threshold it was solved for.

```csharp
using Lodestar.Text.Similarity;

LshBanding banding = LshBanding.Solve(0.8, 128);

double above = banding.CollisionProbability(0.9);  // => 0.9286…
double below = banding.CollisionProbability(0.7);  // => 0.0838…
```

**Remarks** — this is what makes a banding legible before an index is built. A pair at 0.9
similarity is found nine times in ten and a pair at 0.7 fewer than one time in ten, which is the
separation a threshold of 0.8 is asking for — and reading those two numbers is faster than
measuring the index afterwards.

The curve is exact, not an estimate: it is the probability given the signatures, and the only
approximation in the pipeline is [`MinHash.Jaccard`](minhash-jaccard.md)'s estimate of `s` itself.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LshBanding.Solve`](lshbanding-solve.md).
