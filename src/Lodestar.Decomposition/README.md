# Lodestar.Decomposition

Truncated SVD and non-negative matrix factorization over a sparse matrix, without
centring it: latent semantic analysis on a term-document matrix, with the explained variance, and
NMF for a parts-based decomposition. Randomized SVD with all three power-iteration normalizers,
the Householder QR, and the variance principal components explain. The dense kernels are written
here, so the only dependency is `Lodestar.Abstractions`.

## Install

```bash
dotnet add package Lodestar.Decomposition
```

## Example

```csharp
using Lodestar.Abstractions;
using Lodestar.Decomposition;

// Row-major CSR: values, the column each sits in, and where each row starts.
CsrMatrix matrix = new(
    4, 3,
    [3.0, 1.0, 2.0, 1.0, 1.0, 4.0, 2.0, 3.0],
    [0, 1, 0, 2, 1, 2, 0, 2],
    [0, 2, 4, 6, 8]);

TruncatedSvd fitted = TruncatedSvd.Fit(matrix, 2);
double first = fitted.SingularValues[0];   // 5.614…
```

## Parity

Replayed against `sklearn.decomposition.TruncatedSVD` and `NMF`.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Abstractions` 0.2.0 or later

## Documentation

- Guide: [decomposition](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/decomposition.md)
- Reference: [decomposition/factorization](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/decomposition/factorization.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Decomposition/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Decomposition/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
