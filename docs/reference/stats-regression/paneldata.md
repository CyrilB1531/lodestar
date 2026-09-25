# Panel data — `Lodestar.Stats.Regression.Panel`

The data types [`PanelRegression`](panel/panelregression.md) takes and returns. They are compiled
into `Lodestar.Abstractions`, as every public data type of the packages is
([decision 0003](../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)), in their
own namespace: the covariances here are `linearmodels`' panel four, clustered by entity or period
and Driscoll-Kraay's, not the instrumental-variables four.

## Types

| Type | What it is |
| --- | --- |
| [`PanelDesign`](paneldata/paneldesign.md) | The response, the regressors, and each row's entity and period. |
| [`PanelOptions`](paneldata/paneloptions.md) | The effects, the covariance, its clusters and kernel, and the intercept. |
| [`PanelCovarianceType`](paneldata/panelcovariancetype.md) | Unadjusted, robust, clustered or Driscoll-Kraay. |
| [`PanelSummary`](paneldata/panelsummary.md) | The table, the three R², the model tests and the variance components. |

The tests they report are [`WaldTest`](common/waldtest.md)s and the kernels
[`KernelType`](common/kerneltype.md)s, both shared with the instrumental-variables estimators in the
[common types](common.md).

## See also

- [Panel regression](panel.md) — the estimators.
- [Python → C# equivalence](../../equivalence.md).
