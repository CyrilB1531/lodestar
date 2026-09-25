# Lodestar.Stats.Regression

Ordinary, weighted and generalized least squares, generalized linear models and the
multinomial logit, each with the whole inference table statsmodels prints: standard errors, t
statistics, p-values, confidence intervals, R² and adjusted R², the F test and variance inflation
factors, with robust (HC0 to HC3, HAC, cluster) covariances. Instrumental variables — two-stage
least squares, LIML and two-step GMM — with the table, the first-stage diagnostics and the
overidentification tests `linearmodels` prints.

## Install

```bash
dotnet add package Lodestar.Stats.Regression
```

## Example

```csharp
using Lodestar.Stats.Regression;

// x2 is x1 plus a hundredth: two regressors that say almost the same thing.
double[] design = [1.0, 1.01, 2.0, 2.02, 3.0, 2.99, 4.0, 4.01, 5.0, 5.02,
                   6.0, 5.99, 7.0, 7.01, 8.0, 8.02, 9.0, 8.99, 10.0, 10.01];
double[] response = [2.2, 4.1, 6.3, 7.9, 10.2, 12.1, 14.3, 15.9, 18.2, 20.1];

OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, featureCount: 2);
double firstVif = summary.VarianceInflationFactors[0];   // 59483.3…
```

## Parity

Replayed against statsmodels' `OLS`, `WLS`, `GLS`, `GLM` and `MNLogit`, and linearmodels'
`IV2SLS`, `IVLIML` and `IVGMM`.
[`docs/equivalence.md`](https://github.com/CyrilB1531/lodestar/blob/main/docs/equivalence.md) maps each Python call to its C#
counterpart, with every deliberate divergence.

## Dependencies

A core package ([decision 0003](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)),
built for `net10.0` and `netstandard2.0`:

- `Lodestar.Stats` 0.5.0 or later
- `Lodestar.Decomposition` 0.3.0 or later
- `Lodestar.Abstractions` 0.2.1 or later

## Documentation

- Guide: [regression inference](https://github.com/CyrilB1531/lodestar/blob/main/docs/guides/regression-inference.md)
- Reference: [stats-regression/glm](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-regression/glm.md)
- Reference: [stats-regression/gls](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-regression/gls.md)
- Reference: [stats-regression/instrumental](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-regression/instrumental.md)
- Reference: [stats-regression/iv](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-regression/iv.md)
- Reference: [stats-regression/mnlogit](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-regression/mnlogit.md)
- Reference: [stats-regression/ols](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-regression/ols.md)
- Reference: [stats-regression/wls](https://github.com/CyrilB1531/lodestar/blob/main/docs/reference/stats-regression/wls.md)
- [Changelog](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Stats.Regression/CHANGELOG.md)
- [Performance](https://github.com/CyrilB1531/lodestar/blob/main/src/Lodestar.Stats.Regression/performance.md)
- [All packages](https://github.com/CyrilB1531/lodestar/blob/main/README.md)
