# NelsonAalenCurve.Equals

Compares the steps and the cumulative hazard, element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(NelsonAalenCurve other)
```

**Parameters** — `other` is the curve to compare against, or `null`.

**Returns** — `true` when the steps and the hazard hold the same values.

**Example** — the same curve, built twice.

```csharp
using Lodestar.Survival;

NelsonAalenCurve left = new([new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [0.0, 0.25]);
NelsonAalenCurve right = new([new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [0.0, 0.25]);

bool same = left == right;  // => True
```

**Remarks** — a record's generated equality compares both arrays by reference, so the two above
would be unequal without this. [Decision
0113](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0113-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NelsonAalenCurve.GetHashCode`](nelsonaalencurve-gethashcode.md),
[`NelsonAalenCurve`](nelsonaalencurve.md).
