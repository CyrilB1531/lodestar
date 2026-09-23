# AndersonResult.GetHashCode

Hashes the scalars and the table length, which is O(1).

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash of the statistic, the p-value and the number of critical values.

**Example** — equal results hash equally, which is the contract a dictionary needs.

```csharp
using Lodestar.Stats;

AndersonResult left = new(0.5, 0.1, [0.5, 0.6], [15.0, 10.0]);
AndersonResult right = new(0.5, 0.1, [0.5, 0.6], [15.0, 10.0]);

bool agree = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the tables are not walked. Equal results necessarily agree on the table length, so
the contract holds; unequal ones may collide, which a hash is allowed to do. Walking both tables
would make the cheap operation cost what the test itself cost, and
[`Chi2ContingencyResult.GetHashCode`](chi2contingencyresult-gethashcode.md) is written the same
way.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AndersonResult`](andersonresult.md),
[`AndersonResult.Equals`](andersonresult-equals.md), the
[Python equivalence table](../../../equivalence.md).
