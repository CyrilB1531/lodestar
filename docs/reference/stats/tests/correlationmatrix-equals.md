# CorrelationMatrix.Equals

Compares the size and both matrices, value by value.

<!-- docs-declaration -->

```csharp
public bool Equals(CorrelationMatrix other)
```

**Parameters** — `other` is the result to compare against.

**Returns** — `true` when both hold the same size and the same numbers, `NaN` included.

**Example** — two matrices computed apart are equal.

```csharp
using Lodestar.Stats;

// Six observations of three variables, row by row.
double[] data =
[
    1.2, 3.4, 10.0,
    2.3, 3.1, 8.0,
    3.1, 4.8, 9.5,
    4.0, 4.2, 6.1,
    5.5, 6.9, 4.0,
    6.1, 6.0, 3.3,
];

bool same = Spearman.Matrix(data, 3).Equals(Spearman.Matrix(data, 3));   // => True
```

**Remarks** — the generated equality would compare the arrays by reference, so two results holding
the same numbers would be unequal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CorrelationMatrix`](correlationmatrix.md).
