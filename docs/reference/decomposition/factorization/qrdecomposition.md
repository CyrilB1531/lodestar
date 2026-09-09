# QrDecomposition

A thin QR factorization of a dense matrix, by Householder reflections.

<!-- docs-declaration -->

```csharp
public sealed class QrDecomposition
```

**Properties** — `RowCount` and `ColumnCount` are the shape of the matrix that was factorized.
`Q` is the orthonormal factor, row-major and `RowCount × ColumnCount`; `R` is the upper-triangular
factor, row-major and `ColumnCount` square.

**Example** — factorize, then read the factors back.

```csharp
using Lodestar.Decomposition;

// A 3 x 2 matrix, row-major.
double[] matrix = [1.0, 2.0, 3.0, 4.0, 5.0, 7.0];

QrDecomposition qr = QrDecomposition.Householder(matrix, rowCount: 3, columnCount: 2);

int q = qr.Q.Count;             // => 6
int r = qr.R.Count;             // => 4
double belowDiagonal = qr.R[2]; // => 0
```

**Remarks** — **thin, not full.** For an `m × n` matrix with `m ≥ n`, `Q` is `m × n` with
orthonormal columns rather than `m × m`, and `R` is `n × n`. That is the shape a least-squares
solve wants, and the one `numpy.linalg.qr(mode="reduced")` returns.

This package writes its own QR because Math.NET was refused on freshness
([`decisions/0059`](../../../decisions/0059-phase-0-verifications-two-confirmed-voids-do-not-survive-nuget.md)),
and the kernel is published now because a second package needs it rather than a second copy of it.
The LU and the one-sided Jacobi SVD beside it stay internal — nothing has asked for them.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`QrDecomposition.Householder`](qrdecomposition-householder.md),
[`TruncatedSvd`](truncatedsvd.md).

## Members

| Member | What it does |
| --- | --- |
| [`QrDecomposition.Householder`](qrdecomposition-householder.md) | Factorizes a row-major matrix by Householder reflections. |
