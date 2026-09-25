# Lodestar.Conformal

Conformal prediction: turn a point prediction into an interval, or a predicted class into a
prediction set, with a finite-sample coverage guarantee. It computes the calibration quantile from
a model's scores on held-out data and applies it at prediction time; the model itself stays
whatever you already use. Without a calibration set to spare, CV+, Jackknife+ and the
jackknife-after-bootstrap score each training sample with a model fitted without it, and the gamma
score gives intervals proportional to a positive prediction. The guarantee assumes the data are
exchangeable.

## Install

```bash
dotnet add package Lodestar.Conformal
```

## Example

```csharp
using Lodestar.Conformal;

// Absolute residuals of a model on a calibration set.
double[] scores = [0.2, 0.1, 0.4, 0.3, 0.5, 0.1, 0.4, 0.3, 0.1];

double q = SplitConformal.Quantile(scores, 0.2);   // 0.4
(double Lower, double Upper) interval = SplitConformal.Interval(11.0, q);   // (10.6, 11.4)
```

## Parity

Replayed against MAPIE's split conformal regressor and classifier, its cross-conformal and
jackknife-after-bootstrap regressors, and its gamma conformity score.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Abstractions` 0.2.1 or later

## Documentation

- Guide: [conformal](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/conformal.md)
- Reference: [conformal/prediction](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/conformal/prediction.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Conformal/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Conformal/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
