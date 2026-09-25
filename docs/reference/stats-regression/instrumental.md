# Instrumental-variables data — `Lodestar.Stats.Regression.Instrumental`

The data types [`InstrumentalVariables`](iv/instrumentalvariables.md) takes and returns. They are
compiled into `Lodestar.Abstractions`, as every public data type of the packages is
([decision 0003](../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)), in their
own namespace so that none of them borrows a name from ordinary least squares: the covariances here
are `linearmodels`' four, not `statsmodels`' seven.

## Types

| Type | What it is |
| --- | --- |
| [`IvDesign`](instrumental/ivdesign.md) | The response and the three blocks: exogenous, endogenous, instruments. |
| [`IvOptions`](instrumental/ivoptions.md) | The covariance, its kernel and scaling, Fuller's `α`, GMM's weight and the intercept. |
| [`IvCovarianceType`](instrumental/ivcovariancetype.md) | Unadjusted, robust, kernel or clustered. |
| [`IvKernel`](instrumental/ivkernel.md) | Bartlett, Parzen or quadratic spectral. |
| [`IvSummary`](instrumental/ivsummary.md) | The table, R², the model test, the first stage and the overidentification test. |
| [`IvFirstStage`](instrumental/ivfirststage.md) | One endogenous regressor's first-stage diagnostics. |
| [`IvTest`](instrumental/ivtest.md) | A statistic, its p-value and its degrees of freedom. |

## See also

- [Instrumental variables](iv.md) — the estimators.
- [Python → C# equivalence](../../equivalence.md).
