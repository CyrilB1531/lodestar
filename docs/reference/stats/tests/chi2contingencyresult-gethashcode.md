# Chi2ContingencyResult.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — a hash over the scalars and the number of rows.

**Example** — equal results hash alike.

```csharp
using Lodestar.Stats;

Chi2ContingencyResult left = new(1.5, 0.2, 1, [[1.0, 2.0], [3.0, 4.0]]);
Chi2ContingencyResult right = new(1.5, 0.2, 1, [[1.0, 2.0], [3.0, 4.0]]);

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the table contributes its row count, never its frequencies. Equal results agree on
that count, and walking the table would make hashing cost what the test itself cost.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Chi2ContingencyResult.Equals`](chi2contingencyresult-equals.md),
[`Chi2ContingencyResult`](chi2contingencyresult.md).
