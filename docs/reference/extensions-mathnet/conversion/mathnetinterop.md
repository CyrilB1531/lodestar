# MathNetInterop

Converts between `CsrMatrix` and Math.NET Numerics' sparse matrix.

<!-- docs-declaration -->

```csharp
public static class MathNetInterop
```

**Example** — a matrix out of Lodestar, into Math.NET, and back.

```csharp
using Lodestar.Abstractions;
using Lodestar.Extensions.MathNet;
using MathNet.Numerics.LinearAlgebra.Double;

var matrix = new CsrMatrix(
    rowCount: 2,
    columnCount: 3,
    values: [1.0, 2.0, 3.0],
    columnIndices: [0, 2, 1],
    rowPointers: [0, 2, 3]);

SparseMatrix sparse = MathNetInterop.ToSparseMatrix(matrix);
double corner = sparse[0, 2];              // => 2

CsrMatrix back = MathNetInterop.ToCsrMatrix(sparse);
int stored = back.NonZeroCount;            // => 3
```

**Remarks** — the two sides agree on the layout, so a compressed-row matrix moves as three arrays
rather than `rows × columns` cells; a dense or diagonal Math.NET matrix is walked over what it
stores. Neither result shares an array with its source: Math.NET's storage
is mutable through `At`, and a shared buffer would let a write on one matrix move the other.

**One asymmetry is worth knowing.** `CsrMatrix` validates four things — the row pointers start at
zero, do not decrease, end at the stored count, and every column index is in range — and says
nothing about the **order** of column indices within a row, about explicit zeros, or about a column
appearing twice. Math.NET reaches a cell by searching the row, which assumes the order. Its own
compressed-row factory sorts each row of its copy and adds duplicate columns, but with a sort that
is unstable past 16 entries, so [`MathNetInterop.ToSparseMatrix`](mathnetinterop-tosparsematrix.md)
bypasses it: a matrix with strictly increasing rows is copied as it is, and any other sorted here
first, a repeated column added in stored order as `CsrMatrix.ToDense` does
([#1548](https://github.com/CyrilB1531/lodestar/issues/1548)).

**Applies to** — net10.0, netstandard2.0.

**See also** — the [matrix conversion index](../conversion.md),
[sparse matrices](../../abstractions/sparse.md),
[matrix factorization](../../decomposition/factorization.md).

## Members

| Member | What it does |
| --- | --- |
| [`MathNetInterop.ToCsrMatrix`](mathnetinterop-tocsrmatrix.md) | Converts a Math.NET matrix into a `CsrMatrix`. |
| [`MathNetInterop.ToSparseMatrix`](mathnetinterop-tosparsematrix.md) | Converts a `CsrMatrix` into a Math.NET `SparseMatrix`. |
