# NelsonAalenCurve.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public override int GetHashCode()
```

**Returns** — a hash over the number of steps.

**Example** — equal curves hash alike.

```csharp
using Lodestar.Survival;

NelsonAalenCurve left = new([new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [0.0, 0.25]);
NelsonAalenCurve right = new([new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [0.0, 0.25]);

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — both arrays share one index, so the step count stands for both. Equal curves agree
on it; unequal ones are allowed to collide rather than pay for a walk.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NelsonAalenCurve.Equals`](nelsonaalencurve-equals.md),
[`NelsonAalenCurve`](nelsonaalencurve.md).
