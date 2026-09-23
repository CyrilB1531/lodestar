# AndersonResult.Equals

Compares the two scalars and both tables, value by value.

<!-- docs-declaration -->

```csharp
public bool Equals(AndersonResult other)
```

**Parameters** — `other` is the result to compare against, or `null`.

**Returns** — `true` when the statistic and the p-value match and both tables hold the same
values in the same order.

**Example** — the same result, built twice.

```csharp
using Lodestar.Stats;

AndersonResult left = new(0.5, 0.1, [0.5, 0.6], [15.0, 10.0]);
AndersonResult right = new(0.5, 0.1, [0.5, 0.6], [15.0, 10.0]);

bool same = left == right;  // => True
```

**Remarks** — written rather than generated because the two members are arrays, which a record's
generated equality compares by reference: two results holding the same critical values would
otherwise be unequal. [`Chi2ContingencyResult.Equals`](chi2contingencyresult-equals.md) exists for
the same reason.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AndersonResult`](andersonresult.md),
[`AndersonResult.GetHashCode`](andersonresult-gethashcode.md), the
[Python equivalence table](../../../equivalence.md).
