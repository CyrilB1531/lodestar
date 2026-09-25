# Panel regression — `Lodestar.Stats.Regression`

One entry point, [`PanelRegression`](panel/panelregression.md), for data that follows the same
entities over several periods: firms by year, patients by visit, regions by month. An entity's
unobserved, time-invariant traits bias least squares when they move with the regressors; the four
estimators here each deal with them differently — projecting them out, averaging them, differencing
them away, or treating them as a random draw — and each reports the table `linearmodels` does,
under a covariance that lets errors correlate within an entity, within a period, or along time.

## Why this exists

No .NET library publishes a panel estimator: Math.NET Numerics, Accord and ML.NET fit pooled least
squares and stop
([decision 0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md)). The
estimators are closed forms over transformed rows, so they are replayed against `linearmodels` at
`1e-9` from a frozen corpus.

## Types

| Type | What it is |
| --- | --- |
| [`PanelRegression`](panel/panelregression.md) | Fits fixed effects, between, first-difference and random-effects regressions. |

The data it takes and returns — [`PanelDesign`](paneldata/paneldesign.md),
[`PanelOptions`](paneldata/paneloptions.md), [`PanelSummary`](paneldata/panelsummary.md) and
[`PanelCovarianceType`](paneldata/panelcovariancetype.md) — live in
[`Lodestar.Stats.Regression.Panel`](paneldata.md).

## See also

- [Ordinary least squares](ols.md) — the pooled regression, without the panel's structure.
- [Shared regression types](common.md) — the tests and kernels.
- [Python → C# equivalence](../../equivalence.md).
