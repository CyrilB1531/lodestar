# CoxBaseline.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash over the stratum label and the number of times.

**Example** — equal baselines hash alike.

```csharp
using Lodestar.Survival;

CoxBaseline left = new(1, [1.0, 2.0], [0.1, 0.2], [0.1, 0.3], [0.9, 0.74]);
CoxBaseline right = new(1, [1.0, 2.0], [0.1, 0.2], [0.1, 0.3], [0.9, 0.74]);

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the four arrays share one index, so the time count stands for all of them; unequal
baselines may collide.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxBaseline.Equals`](coxbaseline-equals.md), [`CoxBaseline`](coxbaseline.md).
