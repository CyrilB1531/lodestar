# KaplanMeierCurve.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash over the confidence level and the number of steps.

**Example** — equal curves hash alike.

```csharp
using Lodestar.Survival;

SurvivalStep[] steps = [new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)];
KaplanMeierCurve left = new(steps, [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.95);
KaplanMeierCurve right = new(
    [new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.95);

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the four arrays share one index, so the step count stands for all of them. A fitted
curve can hold thousands of steps, which is the walk this avoids; unequal curves may collide.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KaplanMeierCurve.Equals`](kaplanmeiercurve-equals.md),
[`KaplanMeierCurve`](kaplanmeiercurve.md).
