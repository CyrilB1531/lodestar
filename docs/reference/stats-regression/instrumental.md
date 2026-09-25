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
| [`IvSummary`](instrumental/ivsummary.md) | The table, R², the model test, the first stage and the overidentification test. |
| [`IvFirstStage`](instrumental/ivfirststage.md) | One endogenous regressor's first-stage diagnostics. |

The tests they report are [`WaldTest`](common/waldtest.md)s and the kernels [`KernelType`](common/kerneltype.md)s,
both shared with the panel estimators in the [common types](common.md).

## See also

- [Instrumental variables](iv.md) — the estimators.
- [Python → C# equivalence](../../equivalence.md).
