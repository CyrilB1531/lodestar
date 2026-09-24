# Lodestar.Stats

Classical hypothesis tests with scipy.stats' semantics: Student and Welch t, Mann-Whitney
U, Wilcoxon signed-rank, chi-square and its contingency form, Fisher exact, Kolmogorov-Smirnov,
Shapiro-Wilk, Anderson-Darling, one-way ANOVA, Kruskal-Wallis, Levene, Bartlett, Friedman, the
binomial test and the correlation tests. Each returns its statistic and p-value, with scipy's
alternatives and `nan_policy`. It also publishes the four distribution tails its neighbours
need.

## Install

```bash
dotnet add package Lodestar.Stats
```

## Example

```csharp
using Lodestar.Stats;

double[] before = [102.0, 98.0, 110.0, 105.0, 99.0];
double[] after = [95.0, 92.0, 99.0, 91.0, 97.0];

TTestResult result = TTest.Independent(before, after);   // Welch's test by default
bool significant = result.PValue < 0.05;                // True
```

## Parity

Replayed against `scipy.stats`.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Abstractions` 0.2.0 or later

## Documentation

- Guide: [hypothesis testing](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/hypothesis-testing.md)
- Reference: [stats/nanpolicy](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats/nanpolicy.md)
- Reference: [stats/tails](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats/tails.md)
- Reference: [stats/tests](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats/tests.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Stats/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Stats/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
