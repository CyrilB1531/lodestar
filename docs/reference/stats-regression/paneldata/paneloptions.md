# PanelOptions

What a panel fit estimates, and how it reports it.

<!-- docs-declaration -->

```csharp
public sealed record PanelOptions
```

**Properties** — `WithIntercept` prepends a constant column to the regressors; `true` by default,
and it has no counterpart in the reference, where a constant is a column the caller supplies.
`EntityEffects` and `TimeEffects` are fixed effects' own; `false` by default.
[`CovarianceType`](panelcovariancetype.md) is the covariance of the estimates; `Unadjusted` by
default, the reference's. `Debiased` counts the coefficients out of the covariance's degrees of
freedom and reads the tests against t and F rather than the normal and χ²; `true` by default, the
reference's. `ClusterEntity` and `ClusterTime` cluster by the rows' entity and period.
[`Kernel`](../common/kerneltype.md) and `Bandwidth` are Driscoll-Kraay's; Bartlett's and `null` by
default, `null` choosing `⌊4(T/100)^(2/9)⌋`. `ConfidenceLevel` is the intervals' level, strictly
inside (0, 1); 0.95 by default.

**Example** — Driscoll-Kraay at the default and at a chosen bandwidth.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];

var design = new PanelDesign(y, x, 1, entities, periods);

var kernel = new PanelOptions { EntityEffects = true, CovarianceType = PanelCovarianceType.Kernel };
PanelSummary automatic = PanelRegression.FixedEffects(design, kernel);
PanelSummary chosen = PanelRegression.FixedEffects(design, kernel with { Bandwidth = 2 });

int? lags = automatic.Bandwidth;                 // => 1
double error = automatic.StandardErrors[1];      // => 0.0228748828…
double wider = chosen.StandardErrors[1];         // => 0.0228760370…
```

**Remarks** — the estimators check the options, not the record: a negative bandwidth, an undeclared
covariance or kernel, and a confidence level outside (0, 1) are refused by the fit, and so is **any
option that fit would not read** — effects outside fixed effects, cluster settings without the
clustered covariance, a kernel or bandwidth without Driscoll-Kraay. A Bartlett or Parzen bandwidth
of `T` or more is refused as the reference refuses it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PanelRegression`](../panel/panelregression.md), [`PanelSummary`](panelsummary.md).
