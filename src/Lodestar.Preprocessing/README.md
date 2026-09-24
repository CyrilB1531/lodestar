# Lodestar.Preprocessing

Feature preprocessing with scikit-learn's semantics: the standard, min-max, max-abs and
robust scalers, the quantile and power transformers, one-hot encoding, binning, imputation
(simple and k-nearest-neighbour), polynomial features and the cross-validation splitters. Each is
fitted on arrays or a `CsrMatrix` and applied to spans, with its fitted statistics readable.

## Install

```bash
dotnet add package Lodestar.Preprocessing
```

## Example

```csharp
using Lodestar.Preprocessing;

// Row-major: two features per row, three rows. The second is constant.
double[] samples = [1.0, 10.0, 2.0, 10.0, 4.0, 10.0];

StandardScaler scaler = StandardScaler.Fit(samples, featureCount: 2);
double[] standardised = scaler.Transform(samples);
double first = standardised[0];   // -1.069…
```

## Parity

Replayed against `sklearn.preprocessing` and `sklearn.model_selection`.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Stats` 0.5.0 or later
- `Lodestar.Abstractions` 0.2.0 or later
- `Lodestar.Cluster` 0.2.0 or later

## Documentation

- Reference: [preprocessing/encoding](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/preprocessing/encoding.md)
- Reference: [preprocessing/scaling](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/preprocessing/scaling.md)
- Reference: [preprocessing/splitting](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/preprocessing/splitting.md)
- Reference: [preprocessing/transforming](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/preprocessing/transforming.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Preprocessing/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Preprocessing/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
