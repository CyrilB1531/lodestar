# Lodestar.Metrics

Evaluation metrics with scikit-learn's exact semantics: classification (precision,
recall, F1, F-beta, the confusion matrix, the classification report, ROC-AUC, balanced accuracy,
Matthews correlation, Cohen's kappa), regression, clustering and ranking metrics. Averaging, the
positive label and `zero_division` behave as scikit-learn documents them, warnings included as a
typed exception when asked for.

## Install

```bash
dotnet add package Lodestar.Metrics
```

## Example

```csharp
using Lodestar.Metrics;

int[] yTrue = [1, 1, 1, 1, 0, 0, 0, 0];
int[] yPred = [1, 1, 0, 0, 1, 0, 0, 0];

double f1 = F1.Score(yTrue, yPred);   // 0.5714…
```

## Parity

Replayed against `sklearn.metrics`.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Abstractions` 0.2.0 or later

## Documentation

- Guide: [metrics](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/metrics.md)
- Reference: [metrics/classification](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/metrics/classification.md)
- Reference: [metrics/clustering](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/metrics/clustering.md)
- Reference: [metrics/ranking](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/metrics/ranking.md)
- Reference: [metrics/regression](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/metrics/regression.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Metrics/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Metrics/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
