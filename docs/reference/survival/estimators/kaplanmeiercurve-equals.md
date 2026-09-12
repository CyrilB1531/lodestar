# KaplanMeierCurve.Equals

Compares the steps and all three curves, element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(KaplanMeierCurve? other)
```

**Parameters** — `other` is the curve to compare against, or `null`.

**Returns** — `true` when the confidence level matches and the steps, estimates and both bounds
hold the same values.

**Example** — the same curve, built twice.

```csharp
using Lodestar.Survival;

SurvivalStep[] steps = [new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)];
KaplanMeierCurve left = new(steps, [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.95);
KaplanMeierCurve right = new(
    [new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.95);

bool same = left == right;  // => True
```

**Remarks** — `SurvivalStep` is a record of value types, so its own equality is already correct;
what needed writing is the comparison of the arrays holding it. [Decision
0112](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0112-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KaplanMeierCurve.GetHashCode`](kaplanmeiercurve-gethashcode.md),
[`KaplanMeierCurve`](kaplanmeiercurve.md).
