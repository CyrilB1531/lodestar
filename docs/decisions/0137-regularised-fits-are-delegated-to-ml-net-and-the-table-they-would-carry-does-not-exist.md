---
status: accepted
supersedes: []
amends: []
applies: ["0074", "0075", "0095", "0096", "0104", "0110", "0111"]
---
# 0137 — Regularised fits are delegated to ML.NET, and the table they would carry does not exist

**Status:** accepted · **Date:** 2026-09-16 · **Applies:** [`0074`](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md), [`0075`](0075-double-metaphone-takes-doublemetaphone-as-its-oracle.md), [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md), [`0096`](0096-ordinary-least-squares-earns-its-own-package.md), [`0104`](0104-generalized-linear-models-are-written-natively.md), [`0110`](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md), [`0111`](0111-the-generalized-linear-model-does-not-earn-its-own-package.md)

## Context

[0104](0104-generalized-linear-models-are-written-natively.md) wrote the GLM natively and left regularised fits out of
its first lot. [#789](https://github.com/CyrilB1531/lodestar/issues/789) took them, and set three questions before any
code: what `statsmodels` reproduces, what its output actually is, and what .NET already has.

The third is what decides this record. This project's thesis is that the gap in .NET is **the apparatus around a
computation** — the inference table, not the estimate ([0096](0096-ordinary-least-squares-earns-its-own-package.md)).
`GLM.fit_regularized` has no inference table to reproduce.

## What the reference returns

`statsmodels` 0.15.0, `GLM.fit_regularized(method="elastic_net", alpha=, L1_wt=, refit=)`. It minimises

`−loglike/n + alpha·((1 − L1_wt)·‖β‖²/2 + L1_wt·‖β‖₁)`

by coordinate descent, and returns a `RegularizedResults` whose whole surface is `params`, `fittedvalues`,
`converged`, `predict` and `summary`. **There is no `bse`**: reading it raises `AttributeError`, and so there are no z
statistics, p-values or intervals. The docstring says why the table is missing rather than merely unimplemented:
*"Post-estimation results are based on the same data used to select variables, hence may be subject to overfitting
biases."*

`refit=True` returns an ordinary `GLMResults` — the unpenalised fit **on the variables the penalty kept**, with zeros
left at zero. That table is [`OrdinaryLeastSquares.Fit`](../reference/stats-regression/ols/ordinaryleastsquares-fit.md)
or [`GeneralizedLinearModel.Fit`](../reference/stats-regression/glm/generalizedlinearmodel-fit.md) on the selected
columns, which this package already publishes.

Measured on 300 rows, six regressors and a binomial family:

| question | measurement |
| --- | ---: |
| default `cnvrg_tol` against `1e-14`, L1_wt 1.0 / 0.5 / 0.0 | 3.4e-10 / 1.6e-10 / 0.0 |
| against `sklearn.linear_model.ElasticNet` (Gaussian lasso) | 8.3e-12 |
| against `sklearn.linear_model.LogisticRegression(penalty="l1", solver="saga")` | 6.5e-10 |

**The estimator is well defined and reproducible.** Two independent implementations land on the same optimum, so a
`1e-9` corpus is reachable — unlike [0136](0136-the-multinomial-logit-is-written-and-the-ordered-model-is-not.md)'s
ordered model.

Two further behaviours, measured: the penalty is **not scale-invariant** — rescaling one regressor by 100 moved its
coefficient from 0.5397 to 0.5690 — and `alpha` accepts a vector, which is how the intercept is left unpenalised.

## The .NET side

`tools/survey.cs` ([0110](0110-the-surveyor-is-a-file-based-app-and-names-its-counting-basis.md)), pattern
`(ElasticNet|Lasso|Ridge|Regulari|Penalt|L1Weight|L2Weight|CoordinateDescent|ProximalGradient|SoftThreshold)`:

| package | what matches | penalty |
| --- | --- | --- |
| `Microsoft.ML` 5.0.0 (MIT, first-party) | `L1Regularization` and `L2Regularization` on `LbfgsLogisticRegression`, `LbfgsPoissonRegression`, `LbfgsMaximumEntropy` and the SDCA trainers | **elastic net** |
| `Numerics.NET` 10.7.0 (commercial) | `RegularizedRegressionModel` with `RegularizationParameter`, `RegularizationRatio`, `Standardize`, `GetRegularizationPath` | elastic net, read only |
| `Accord.Statistics` 3.8.0 (LGPL, archived) | `IterativeReweightedLeastSquares.Regularization`, `LogisticRegressionAnalysis.Regularization` | L2 only |
| `NumFlat` 1.3.4 | `LogisticRegressionOptions.Regularization` | L2 only |
| `Dew.Stats.Core` 6.3.10 (commercial) | `RidgeRegress`, `RidgeOptimalk` | L2 only |
| `MathNet.Numerics` 5.0.0, `Meta.Numerics` 4.2.0, `ILNumerics.Toolboxes.Statistics` 7.4.62 | the incomplete beta and gamma functions | none |

**ML.NET fits the same estimator.** Its regularisation weights apply to the *summed* loss where `statsmodels`' `alpha`
applies to the mean, so the two agree once the weight is scaled by `n`. On the same 300 rows, with
`L1Regularization = alpha·L1_wt·n` and `L2Regularization = alpha·(1 − L1_wt)·n`:

| penalty | ML.NET `LbfgsLogisticRegression` | `statsmodels` |
| --- | --- | --- |
| lasso | 0.347496, 0.726758, −0.585131, 0, 0, 0 | 0.347497, 0.726758, −0.585131, 0, 0, 0 |
| ridge | 0.381587, 0.806020, −0.668012, 0.182058, 0.027144, 0.122803 | 0.381594, 0.806037, −0.668038, 0.182065, 0.027138, 0.122788 |
| elastic net, `L1_wt = 0.5` | 0.362476, 0.770978, −0.634260, 0.092317, 0, 0.026859 | 0.362476, 0.770978, −0.634260, 0.092317, 0, 0.026859 |

The same optimum and the same selected variables, zeros included. The residual gap on the ridge fit, `1.8e-5`, is
ML.NET storing its weights as `float`.

## Decision

1. **Regularised fits are not written here. The coefficients are delegated to `Microsoft.ML`'s LBFGS trainers**, with
   the penalty scaled by the row count, as `docs/migration/statsmodels.md` now records. It is first-party, MIT and
   maintained, and it reaches the same optimum as the reference.
2. **The reason is the missing table, not the arithmetic.** What this package adds to a fit is the inference table
   around it, and `fit_regularized` publishes none — deliberately, because the selection and the inference would read
   the same rows. Writing the coefficients alone would duplicate a first-party library to no end (0104's own test for
   delegation).
3. **The table on the selected support is already reachable**, and the migration page carries the two-line recipe:
   fit the penalised model, then refit the non-zero columns through
   [`OrdinaryLeastSquares.Fit`](../reference/stats-regression/ols/ordinaryleastsquares-fit.md) or
   [`GeneralizedLinearModel.Fit`](../reference/stats-regression/glm/generalizedlinearmodel-fit.md). That is what
   `refit=True` computes, with the same caveat the reference prints.
4. **What would reopen this**, under [0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s
   rule, is a caller who needs one of the three things delegation does not give:
   - **double precision** — ML.NET's weights are `float`, `1.8e-5` from the reference here;
   - **no `IDataView`** — its trainers are welded to it, the coupling this project's README already indicts for
     metrics;
   - **post-selection inference** proper, which no reference here publishes and which is a research question rather
     than a port.

### Rejected

- **Writing the coefficient path anyway.** It is a solved computation in a maintained first-party package; 0104
  delegated to ML.NET for less.
- **Publishing `refit=True`'s table as a regularised fit's table.** It is an unpenalised fit reported under a
  penalised name, and the reference warns about exactly that reading.
- **Delegating to `Numerics.NET`.** Commercial; read, never run ([0129](0129-four-numerics-libraries-read-and-three-absences-withdrawn.md)).
- **Delegating to Accord or NumFlat.** L2 only, and Accord is LGPL-2.1 and archived (0104).

## Consequences

- #789 closes on this record, with no code.
- `docs/migration/statsmodels.md` gains the delegation row and the refit recipe; `docs/equivalence.md` records that
  `fit_regularized` has no counterpart here and why.
- The GLM extensions umbrella #777 closes when its three sub-issues do, this one included.
