# MathNetInterop.ToCsrMatrix

Converts a Math.NET matrix into a `CsrMatrix`.

<!-- docs-declaration -->

```csharp
public static CsrMatrix ToCsrMatrix(Matrix<double> matrix)
```

**Parameters** — `matrix` is the matrix to convert; sparse or dense.

**Returns** — a `CsrMatrix` of the same shape holding the same values, sharing no array with the
source.

**Exceptions** — `ArgumentNullException` when `matrix` is null.

**Example** — a dense matrix keeps only what it actually stores.

```csharp
using Lodestar.Abstractions;
using Lodestar.Extensions.MathNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

Matrix<double> dense = DenseMatrix.OfRowArrays(
    [0.0, 3.0, 0.0],
    [0.0, 0.0, 0.0],
    [4.0, 0.0, 5.0]);

CsrMatrix csr = MathNetInterop.ToCsrMatrix(dense);

int stored = csr.NonZeroCount;   // => 3
int rows = csr.RowCount;         // => 3
```

**Remarks** — a matrix already in compressed-row form hands over its three arrays, copied so neither
side can mutate the other's. **Any other storage is walked.** A dense matrix, or a compressed-column
one, has none of the arrays the fast path copies, so its cells are visited row by row and the
non-zero ones collected — the honest cost of changing layout, not a defect of this path. A caller
converting a large dense matrix is paying for the layout change and should expect to.

An empty row is a row: it contributes no stored value and its row pointer repeats the previous one,
which is what the example's middle row shows.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MathNetInterop`](mathnetinterop.md),
[`MathNetInterop.ToSparseMatrix`](mathnetinterop-tosparsematrix.md).
