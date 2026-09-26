# SurvivalCurve.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash over the confidence level and the number of steps.

**Example** — equal curves hash alike.

```csharp
using Lodestar.Survival;

SurvivalCurve left = BreslowFlemingHarrington.Estimate([1, 2, 3], [true, false, true]);
SurvivalCurve right = BreslowFlemingHarrington.Estimate([1, 2, 3], [true, false, true]);

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the four arrays share one index, so the step count stands for all of them. A curve can
hold thousands of steps, which is the walk this avoids; unequal curves may collide.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SurvivalCurve.Equals`](survivalcurve-equals.md), [`SurvivalCurve`](survivalcurve.md).
