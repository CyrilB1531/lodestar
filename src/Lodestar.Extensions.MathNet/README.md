# Lodestar.Extensions.MathNet

Converts a `CsrMatrix` to and from Math.NET Numerics' sparse matrix in one pass over the
stored values, so a caller already on Math.NET can hand a Lodestar vectorizer's output to it, or
the other way round, without densifying.

## Install

```bash
dotnet add package Lodestar.Extensions.MathNet
```

## Example

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
int stored = csr.NonZeroCount;   // 3
```

## Parity

A conversion: the values in equal the values out, which the tests assert both ways.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A interop package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Abstractions` 0.2.0 or later
- `MathNet.Numerics` 5.0.0 or later

## Documentation

- Reference: [extensions-mathnet/conversion](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/extensions-mathnet/conversion.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Extensions.MathNet/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Extensions.MathNet/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
