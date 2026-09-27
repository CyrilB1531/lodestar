# ChiSquaredContingencyResult.Equals

Compares the scalars and the expected table, row by row.

<!-- docs-declaration -->

```csharp
public bool Equals(ChiSquaredContingencyResult other)
```

**Parameters** — `other` is the result to compare against, or `null`.

**Returns** — `true` when the statistic, p-value and degrees of freedom match and both expected
tables hold the same frequencies.

**Example** — the same result, reached twice.

```csharp
using Lodestar.Stats;

ChiSquaredContingencyResult left = new(1.5, 0.2, 1, [[1.0, 2.0], [3.0, 4.0]]);
ChiSquaredContingencyResult right = new(1.5, 0.2, 1, [[1.0, 2.0], [3.0, 4.0]]);

bool same = left == right;  // => True
```

**Remarks** — the comparison descends into each row rather than stopping at the row count, which
is what a table differing inside one row needs. the equality rule
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ChiSquaredContingencyResult.GetHashCode`](chisquaredcontingencyresult-gethashcode.md),
[`ChiSquaredContingencyResult`](chisquaredcontingencyresult.md).
