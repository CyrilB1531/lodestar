# Lodestar.Abstractions

The types the other Lodestar packages share, and nothing that computes with them beyond
what a sparse matrix needs. `CsrMatrix` is a compressed sparse row matrix with its row norms and
the vector and dense-block products a decomposition runs on; `SparseNorm` names the row norm a
vectorizer applies. Since 0.2.0 it also holds the public data types every other package declares
(options, results, enums), each under its original namespace and forwarded from the package that
declared it, so upgrading one package never duplicates a type (decision 0003).

Most applications reach it through another package rather than install it directly.

## Install

```bash
dotnet add package Lodestar.Abstractions
```

## Example

```csharp
using Lodestar.Abstractions;

// Two rows, three columns: row 0 holds 1 at column 0 and 2 at column 2; row 1 holds 3 at column 1.
CsrMatrix matrix = CsrMatrix.CreateUnchecked(2, 3, [1.0, 2.0, 3.0], [0, 2, 1], [0, 2, 3]);

int rows = matrix.RowCount;             // 2
double firstRow = matrix.RowL1Norm(0);  // 3
```

## Parity

`CsrMatrix` follows `scipy.sparse.csr_matrix`'s layout: values, column indices and row
pointers, in that order.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- none

## Documentation

- Reference: [abstractions/sparse](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/abstractions/sparse.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Abstractions/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Abstractions/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
