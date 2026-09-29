# MathNetInterop.ToSparseMatrix

Converts a `CsrMatrix` into a Math.NET `SparseMatrix`.

<!-- docs-declaration -->

```csharp
public static SparseMatrix ToSparseMatrix(CsrMatrix matrix)
```

**Parameters** — `matrix` is the matrix to convert.

**Returns** — a `SparseMatrix` of the same shape holding the same values, sharing no array with the
source.

**Exceptions** — `ArgumentNullException` when `matrix` is null. `ArgumentException` when it is not a
valid CSR matrix — built through `CsrMatrix.CreateUnchecked`, or its arrays changed after
construction — which Math.NET would answer with an error of its own base type, or accept with a
column outside the matrix ([#1549](https://github.com/CyrilB1531/lodestar/issues/1549)).

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

**Remarks** — **the rows are sorted, and duplicate columns are added together**, because
`CsrMatrix` never promised an order and Math.NET reaches a cell by searching the row. The example
above is exactly that case, and the tests pin it
([#1402](https://github.com/CyrilB1531/lodestar/issues/1402)). A matrix whose rows are already
strictly increasing — what every vectorizer here produces — is copied into Math.NET's storage as
it is; any other is sorted here first, a repeated column summed in stored order as
`CsrMatrix.ToDense` sums it. Math.NET's compressed-row factory is not used: its sort is unstable
past 16 entries in a row, so a row holding `1e16`, `1`, `-1e16` in one column could read `1` where
`ToDense` reads `0`, and it would sort again what is already sorted
([#1548](https://github.com/CyrilB1531/lodestar/issues/1548)).

Explicit zeros are carried across rather than dropped — Math.NET's own compressed-row storage counts
*"stored values including explicit zeros"*, so a matrix with sorted rows and no repeated column
keeps its structure unchanged; an unsorted row comes back sorted, and a repeated column as one
stored sum, zero if the two cancel.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MathNetInterop`](mathnetinterop.md),
[`MathNetInterop.ToCsrMatrix`](mathnetinterop-tocsrmatrix.md).
