# QrDecomposition.Householder

Factorizes a row-major matrix by Householder reflections.

<!-- docs-declaration -->

```csharp
public static QrDecomposition Householder(ReadOnlySpan<double> matrix, int rowCount, int columnCount)
```

**Parameters** — `matrix` is the matrix, row-major: `columnCount` values per row. `rowCount` and
`columnCount` are its shape, and there must be at least as many rows as columns.

**Returns** — the thin factorization.

**Exceptions** — `ArgumentOutOfRangeException` when either dimension is not positive, or when
there are more columns than rows. `ArgumentException` when `matrix` does not hold exactly
`rowCount × columnCount` values.

**Example** — the factors multiply back to the matrix.

```csharp
using Lodestar.Decomposition;

double[] matrix = [1.0, 2.0, 3.0, 4.0, 5.0, 7.0];
QrDecomposition qr = QrDecomposition.Householder(matrix, rowCount: 3, columnCount: 2);

// Q · R reproduces the first entry.
double rebuilt = (qr.Q[0] * qr.R[0]) + (qr.Q[1] * qr.R[2]);   // => 0.9999999999999997

// Q's columns are orthonormal, so the first has unit length.
double lengthSquared = (qr.Q[0] * qr.Q[0]) + (qr.Q[2] * qr.Q[2]) + (qr.Q[4] * qr.Q[4]);
```

**Remarks** — **the reflections are LAPACK's**, `dgeqr2` and `dorg2r`, so the signs are
`numpy.linalg.qr`'s and, on a matrix of full rank, the two agree entry for entry, to rounding. So do
their infinities and NaNs: a matrix holding `±∞` or NaN comes back with the same entries infinite,
NaN or finite as numpy's. On a rank-deficient matrix, past the pivot that vanishes, a column of Q is
built from rounding noise on both sides, and the two agree only in `Q · R`.

A wide matrix is refused rather than padded: there is no thin QR of one, and answering with a full
factorization under a method that promises a thin one would be worse than saying so.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`QrDecomposition`](qrdecomposition.md), [`TruncatedSvd.Fit`](truncatedsvd-fit.md).
