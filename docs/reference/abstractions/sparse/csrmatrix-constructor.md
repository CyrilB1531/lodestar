# CsrMatrix(rowCount, columnCount, values, columnIndices, rowPointers)

Builds a matrix from its three arrays, checking that they describe one: the boundary a matrix read
from a file, a network or a caller crosses.

<!-- docs-declaration -->

```csharp
public CsrMatrix(int rowCount, int columnCount, double[] values, int[] columnIndices, int[] rowPointers)
```

**Parameters** — `rowCount` and `columnCount` are the logical shape, zeros included. `values` holds
the stored cells, row by row, and `columnIndices` the column of each. `rowPointers` holds
`rowCount + 1` entries: where each row starts in `values`, then the stored count.

**Returns** — the matrix, sharing the three arrays: they are taken as they are, not copied.

**Exceptions** — `ArgumentNullException` when an array is `null`. `ArgumentOutOfRangeException`
when `rowCount` or `columnCount` is negative. `ArgumentException` when the arrays do not describe a
matrix: `rowPointers` does not hold `rowCount + 1` entries, `values` and `columnIndices` differ in
length, the first row pointer is not `0`, a pointer decreases, the last is not the stored count, or
a column index is outside `[0, columnCount)`.

**Example** — a 2 × 3 matrix storing three cells.

```csharp
using Lodestar.Abstractions;

var matrix = new CsrMatrix(
    rowCount: 2,
    columnCount: 3,
    values: [1.0, 2.0, 3.0],
    columnIndices: [0, 2, 1],
    rowPointers: [0, 2, 3]);

int stored = matrix.NonZeroCount;     // => 3
double second = matrix.RowL1Norm(1);  // => 3
```

**Remarks** — every refused shape is a case in `CsrMatrixValidationTests`. A column stored twice in
one row is accepted, and read as scipy reads it; [`CsrMatrix`](csrmatrix.md) says which members sum
the two. [`CsrMatrix.CreateUnchecked`](csrmatrix-createunchecked.md) skips the per-entry pass for
arrays valid by construction.

Runs on both target frameworks: net10.0, netstandard2.0.

**See also** — [`CsrMatrix`](csrmatrix.md), [`CsrMatrix.CreateUnchecked`](csrmatrix-createunchecked.md).
