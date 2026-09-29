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
side can mutate the other's. **Any other storage is walked over what it stores.** A dense matrix is
read from its own column-major array and a diagonal one enumerated over its stored entries; either
way the non-zero ones are counted per row and then placed, so a diagonal matrix costs its diagonal,
not its square ([#1220](https://github.com/CyrilB1531/lodestar/issues/1220)), and a dense one its
cells, which is the honest cost of changing layout. On those two paths a stored `NaN` is kept and a
stored zero dropped; the compressed-row path copies explicit zeros with the rest, so a
`CsrMatrix` with sorted rows and no repeated column round-trips unchanged, zeros included
([#1403](https://github.com/CyrilB1531/lodestar/issues/1403)).

An empty row is a row: it contributes no stored value and its row pointer repeats the previous one,
which is what the example's middle row shows.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MathNetInterop`](mathnetinterop.md),
[`MathNetInterop.ToSparseMatrix`](mathnetinterop-tosparsematrix.md).
