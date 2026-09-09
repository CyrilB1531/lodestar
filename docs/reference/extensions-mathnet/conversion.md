# Matrix conversion — `Lodestar.Extensions.MathNet`

One type, [`MathNetInterop`](conversion/mathnetinterop.md): it converts a `CsrMatrix` to and from
Math.NET Numerics' sparse matrix, so a caller who already holds Math.NET types can reach
[`Lodestar.Decomposition`](../decomposition/factorization.md) and the sparse vectorizers without
rebuilding a matrix cell by cell.

Both sides store a matrix in compressed sparse row form and both expose those arrays, so each
direction is one pass over the stored values rather than a walk over every cell. That is the whole
reason this package is worth installing: the conversion anyone can write by hand is the slow one.

## Why this package exists at all

`Lodestar.Decomposition` **refused** Math.NET for its own kernels — 5.0.0 dates from 2022-04-03 and
nothing stable has followed — and writes its own QR, LU and Jacobi SVD instead. Referencing the same
library here is not a reversal:
[`decisions/0089`](../../decisions/0089-the-interop-tier-may-take-a-dependency-a-core-package-refused.md)
separates the two. Computing *with* a stale dependency hands its problems to a caller who never
asked; converting *to* a caller's own types hands them nothing they were not already carrying, and a
caller who holds no Math.NET types installs nothing.

## What is not offered

The dense pair. `CsrMatrix.ToDense()` already returns a `double[,]`, and Math.NET builds a
`DenseMatrix` from one unaided, so a wrapper would add a member that saves nobody a line. The sparse
pair is the one conversion neither side can do for itself.

## Types

| Type | What it is |
| --- | --- |
| [`MathNetInterop`](conversion/mathnetinterop.md) | Converts between `CsrMatrix` and Math.NET's sparse matrix. |

## See also

- [Sparse matrices](../abstractions/sparse.md) — where `CsrMatrix` is documented.
- [Matrix factorization](../decomposition/factorization.md) — what a converted matrix is usually for.
- [Python → C# equivalence](../../equivalence.md).
