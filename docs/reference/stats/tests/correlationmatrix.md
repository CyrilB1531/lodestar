# CorrelationMatrix

A correlation between every pair of variables, and the p-value of each.

<!-- docs-declaration -->

```csharp
public sealed record CorrelationMatrix(int VariableCount, double[] Statistics, double[] PValues)
```

**Properties** — `VariableCount` is the side of both matrices. `Statistics` and `PValues` are
row-major: entry `i × VariableCount + j` relates variable `i` to variable `j`, and the diagonal is
each variable against itself.

**Example** — reading one entry, and its mirror.

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

CorrelationMatrix matrix = Spearman.Matrix(data, 3);
int k = matrix.VariableCount;

double firstThird = Math.Round(matrix.Statistics[(0 * k) + 2], 6);   // => -0.942857
double thirdFirst = Math.Round(matrix.Statistics[(2 * k) + 0], 6);   // => -0.942857
```

**Remarks** — what `scipy.stats.spearmanr` returns for a 2-D array, as two square matrices here
flattened. Equality compares the matrices value by value, as [`AndersonResult`](andersonresult.md)
compares its tables.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Spearman.Matrix`](spearman-matrix.md).

## Members

| Member | What it does |
| --- | --- |
| [`CorrelationMatrix.Equals`](correlationmatrix-equals.md) | Compares the size and both matrices, value by value. |
| [`CorrelationMatrix.GetHashCode`](correlationmatrix-gethashcode.md) | Hashes the size. |
