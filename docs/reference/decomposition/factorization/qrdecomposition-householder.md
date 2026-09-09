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

**Remarks** — **the signs are the reflections', not a convention.** A QR is unique only up to the
sign of each column, and nothing is normalised here. A caller comparing against
`numpy.linalg.qr` should compare `Q · R`, or compare column by column up to sign, rather than
expecting the two to agree entry for entry.

A wide matrix is refused rather than padded: there is no thin QR of one, and answering with a full
factorization under a method that promises a thin one would be worse than saying so.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`QrDecomposition`](qrdecomposition.md), [`TruncatedSvd.Fit`](truncatedsvd-fit.md).
