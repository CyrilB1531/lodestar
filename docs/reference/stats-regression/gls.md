# Generalized least squares — `Lodestar.Stats.Regression`

One entry point. [`GeneralizedLeastSquares.Fit`](gls/generalizedleastsquares-fit.md) fits a linear model
under an error covariance the caller supplies and returns the same [`OlsSummary`](ols/olssummary.md) an
ordinary fit does, at `statsmodels.api.GLS` parity.

**For errors that are correlated**, not only unequal: neighbouring readings in time or space, repeated
measurements of one unit. The rows are whitened by the inverse of the covariance's Cholesky factor and
fitted as [ordinary least squares](ols.md) would. A covariance that is diagonal is
[weighted least squares](wls.md) with weights `1/σ`, which is also how `statsmodels`' vector `sigma` maps here.

## Why this exists

`MathNet.Numerics` 5.0.0 exports no generalized least squares at all, and its weighted regression returns the
coefficients only — the reading
[`decisions/0003`](../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md) recorded.
Decision [0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md)
put `GLS` after the weighted fit and the GLM families.

## Types

| Type | What it is |
| --- | --- |
| [`GeneralizedLeastSquares`](gls/generalizedleastsquares.md) | Fits the model under a given error covariance and builds the table. |

The summary, the options and the covariance of the estimates are [`OlsSummary`](ols/olssummary.md),
[`OlsOptions`](ols/olsoptions.md) and [`CovarianceType`](ols/covariancetype.md).

## See also

- [Regression inference](../../guides/regression-inference.md) — reading the table.
- [statsmodels → .NET](../../migration/statsmodels.md) — what is delegated and what is not.
- [Python → C# equivalence](../../equivalence.md).
