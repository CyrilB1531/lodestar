# MathNetInterop.ToSparseMatrix

Converts a `CsrMatrix` into a Math.NET `SparseMatrix`.

<!-- docs-declaration -->

```csharp
public static SparseMatrix ToSparseMatrix(CsrMatrix matrix)
```

**Parameters** — `matrix` is the matrix to convert.

**Returns** — a `SparseMatrix` of the same shape holding the same values, sharing no array with the
source.

**Exceptions** — `ArgumentNullException` when `matrix` is null.

**Example** — a row whose columns arrive out of order still reads correctly.

```csharp
using Lodestar.Abstractions;
using Lodestar.Extensions.MathNet;
using MathNet.Numerics.LinearAlgebra.Double;

var unsorted = new CsrMatrix(
    rowCount: 1,
    columnCount: 5,
    values: [9.0, 7.0, 8.0],
    columnIndices: [4, 0, 2],
    rowPointers: [0, 3]);

SparseMatrix sparse = MathNetInterop.ToSparseMatrix(unsorted);

double first = sparse[0, 0];    // => 7
double middle = sparse[0, 2];   // => 8
double last = sparse[0, 4];     // => 9
double gap = sparse[0, 3];      // => 0
```

**Remarks** — **the rows are sorted, and duplicate columns are added together**, by Math.NET's
own compressed-row factory, which copies the three arrays and then normalises its copy. That is not
tidiness: `CsrMatrix` never promised an order, and Math.NET reaches a cell by searching the row. The
example above is exactly that case, and the tests pin it, so a Math.NET release that stopped
normalising would fail them rather than answer lookups with zeros
([#1402](https://github.com/CyrilB1531/lodestar/issues/1402)).

Explicit zeros are carried across rather than dropped — Math.NET's own compressed-row storage counts
*"stored values including explicit zeros"*, so a matrix with sorted rows and no repeated column
keeps its structure unchanged; an unsorted row comes back sorted, and a repeated column as one
stored sum, zero if the two cancel.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MathNetInterop`](mathnetinterop.md),
[`MathNetInterop.ToCsrMatrix`](mathnetinterop-tocsrmatrix.md).
