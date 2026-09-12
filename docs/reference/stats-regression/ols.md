# Ordinary least squares — `Lodestar.Stats.Regression`

One entry point, [`OrdinaryLeastSquares.Fit`](ols/ordinaryleastsquares-fit.md): it fits a linear
model and returns what a `statsmodels` summary table holds — the estimates, and how sure it is of
each of them.

**Spans in, one summary out.** A design is row-major, `featureCount` values per row, with no
constant column of your own: `WithIntercept` adds it. That is the shape
[`Lodestar.Metrics`](../metrics/classification.md) and
[`Lodestar.Preprocessing`](../preprocessing/scaling.md) already use, so a matrix crosses between
them without being reshaped.

## Why this exists

The estimate is the cheap half. Read through a `MetadataLoadContext` rather than through a README,
`MathNet.Numerics` 5.0.0 exports 5 333 members and **every regression entry point among them
returns coefficients**: `MultipleRegression.QR`, `.Svd`, `.NormalEquations`, `.DirectMethod`,
`SimpleRegression.Fit`, `WeightedRegression.Weighted`. `GoodnessOfFit` adds five whole-model
scalars. A coefficient covariance matrix does exist in that assembly —
`Optimization.NonlinearMinimizationResult` exports `Covariance`, `Correlation` and
`StandardErrors` — but it belongs to the non-linear minimisers and is unreachable from
`LinearRegression`. Past it there is no t statistic, no p-value, no interval on a coefficient, no
adjusted R-squared, no overall F and no VIF anywhere in the assembly.

`Accord.Statistics` 3.8.0 did have the whole table, and its repository is archived: last release
2017-10-19, last push 2020-11-18, LGPL-2.1. So the gap is not an unexplored one. It is a
maintained, permissively licensed, framework-free OLS table.
[`decisions/0096`](../../decisions/0096-ordinary-least-squares-earns-its-own-package.md) has the
reading and what it decided.

## Types

| Type | What it is |
| --- | --- |
| [`OrdinaryLeastSquares`](ols/ordinaryleastsquares.md) | Fits the model and builds the table. |
| [`OlsSummary`](ols/olssummary.md) | The fitted model, its errors, its p-values and its diagnostics. |
| [`OlsOptions`](ols/olsoptions.md) | Whether to fit an intercept, and at what confidence. |
| [`CovarianceType`](ols/covariancetype.md) | How the covariance of the estimates is estimated. |

## See also

- [Regression inference](../../guides/regression-inference.md) — reading the table.
- [statsmodels → .NET](../../migration/statsmodels.md) — what is delegated and what is not.
- [Python → C# equivalence](../../equivalence.md).
