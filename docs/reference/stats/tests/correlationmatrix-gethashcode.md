# CorrelationMatrix.GetHashCode

Hashes the size, which is O(1).

<!-- docs-declaration -->

```csharp
public int GetHashCode()
```

**Returns** — the variable count: equal results agree on it, unequal ones may collide.

**Example** — the hash is the side of the matrix.

```csharp
using Lodestar.Stats;

CorrelationMatrix matrix = Spearman.Matrix([1.0, 3.0, 2.0, 1.0, 3.0, 2.0], 2);

int hash = matrix.GetHashCode();   // => 2
```

**Remarks** — walking the matrices would make the cheap operation cost what the test cost.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CorrelationMatrix.Equals`](correlationmatrix-equals.md).
