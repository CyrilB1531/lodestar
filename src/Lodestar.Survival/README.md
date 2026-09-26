# Lodestar.Survival

Survival analysis for right-censored data: the Kaplan-Meier survival function with
Greenwood variance and log-log confidence intervals, its restricted mean and fixed-time comparison,
the Nelson-Aalen cumulative hazard, the log-rank family — weighted, multi-group and pairwise — the
concordance index and the Cox proportional-hazards model. No C# implementation of these existed before
this package.

## Install

```bash
dotnet add package Lodestar.Survival
```

## Example

```csharp
using Lodestar.Survival;

// Three subjects: an event at 1, a censoring at 2, an event at 3.
KaplanMeierCurve curve = KaplanMeier.Estimate([1, 2, 3], [true, false, true]);

double afterFirst = curve.Survival[1];   // 0.666…
```

## Parity

Replayed against lifelines.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Stats` 0.5.0 or later
- `Lodestar.Abstractions` 0.2.1 or later

## Documentation

- Guide: [survival analysis](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/survival-analysis.md)
- Reference: [survival/estimators](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/survival/estimators.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Survival/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Survival/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
