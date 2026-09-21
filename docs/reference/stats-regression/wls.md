# Weighted least squares — `Lodestar.Stats.Regression`

One entry point, for a linear model whose rows are not equally trusted. [`WeightedLeastSquares.Fit`](wls/weightedleastsquares-fit.md) fits a linear model
with one weight per row and returns the same [`OlsSummary`](ols/olssummary.md) an ordinary fit does,
at `statsmodels.api.WLS` parity.

**A weight is how much a row is trusted**, proportional to the inverse of its variance: a mean over
forty observations deserves more say than a mean over three. The design, the options and the table
are those of [ordinary least squares](ols.md), so a caller who already reads one reads the other.

## Why this exists

`MathNet.Numerics` 5.0.0 exports `WeightedRegression.Weighted`, and like every regression entry
point in that assembly it returns the coefficients and stops — the reading
[`decisions/0003`](../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md) recorded for
the unweighted fit holds here unchanged. Decision
[0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md)
put the weighted table first after the robust covariances.

## Types

| Type | What it is |
| --- | --- |
| [`WeightedLeastSquares`](wls/weightedleastsquares.md) | Fits the weighted model and builds the table. |

The summary, the options and the covariance choice are [`OlsSummary`](ols/olssummary.md),
[`OlsOptions`](ols/olsoptions.md) and [`CovarianceType`](ols/covariancetype.md).

## See also

- [Regression inference](../../guides/regression-inference.md) — reading the table.
- [Generalized least squares](gls.md) — when the errors are correlated, not only unequal.
- [statsmodels → .NET](../../migration/statsmodels.md) — what is delegated and what is not.
- [Python → C# equivalence](../../equivalence.md).
