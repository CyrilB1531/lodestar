# SurvivalCurve.Equals

Compares the steps, the estimate and both bounds, element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(SurvivalCurve other)
```

**Parameters** — `other` is the curve to compare against, or `null`.

**Returns** — `true` when the confidence level matches and the steps, estimates and both bounds
hold the same values.

**Example** — the same curve, estimated twice.

```csharp
using Lodestar.Survival;

SurvivalCurve left = BreslowFlemingHarrington.Estimate([1, 2, 3], [true, false, true]);
SurvivalCurve right = BreslowFlemingHarrington.Estimate([1, 2, 3], [true, false, true]);

bool same = left == right;  // => True
```

**Remarks** — the equality a record generates would compare the arrays by reference, so two curves
estimated from the same sample would differ. The level is compared by its bits, so a `NaN` level
still equals itself.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SurvivalCurve.GetHashCode`](survivalcurve-gethashcode.md),
[`SurvivalCurve`](survivalcurve.md).
