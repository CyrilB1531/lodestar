# PanelSummary

A panel fit's inference table and diagnostics, as `linearmodels` reports them.

<!-- docs-declaration -->

```csharp
public sealed class PanelSummary
```

**Properties** — the per-coefficient lists run constant, then regressors: `Coefficients`,
`StandardErrors`, `TStatistics`, `PValues`, `ConfidenceLower` and `ConfidenceUpper`.
[`CovarianceType`](panelcovariancetype.md), `Debiased`, `Bandwidth` (Driscoll-Kraay's, as given or
as chosen, else `null`) and `ConfidenceLevel` echo how they were computed. `HasConstant` says
whether the regressors hold a constant. `ObservationCount` is the regression's rows — entities for
between, differences for first-difference — and `EntityCount` and `PeriodCount` the panel's.
`ResidualDegreesOfFreedom` follows. `RSquared` is the regression's own; `RSquaredWithin`,
`RSquaredBetween` and `RSquaredOverall` apply the coefficients to the entity-demeaned rows, the
entity means and the rows as given. `ModelTest` is the homoskedastic F that every coefficient but the
constant is zero, `RobustModelTest` the same under the chosen covariance, and `PoolabilityTest` fixed
effects' F that the effects are zero, all [`WaldTest`](../common/waldtest.md)s or `null`.
`ResidualVariance`, `EffectsVariance` and `Rho` are the variance decomposition, for fixed and random
effects; `Theta` is random effects' per-entity weight.

**Example** — the whole-model half of pooled least squares.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Panel;

int[] entities = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
int[] periods = [2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023, 2020, 2021, 2022, 2023];
double[] x = [0.5, 1.1, 1.9, 2.4, 1.2, 1.8, 2.9, 3.1, -0.3, 0.4, 0.8, 1.6, 2.0, 2.2, 3.1, 3.9];
double[] y = [2.1, 3.0, 4.2, 4.9, 4.4, 5.1, 6.8, 7.0, 0.2, 1.3, 1.7, 3.1, 6.1, 6.3, 7.9, 9.2];

var design = new PanelDesign(y, x, 1, entities, periods);

PanelSummary summary = PanelRegression.FixedEffects(design);

double overall = summary.RSquaredOverall;          // => 0.9287034786…
double between = summary.RSquaredBetween;          // => 0.9588515532…
double within = summary.RSquaredWithin;            // => 0.8127574405…
double f = summary.ModelTest!.Statistic;           // => 182.3630164…
int entityCount = summary.EntityCount;             // => 4
```

**Remarks** — **p-values are the precise tail.** The reference computes `2 − 2·cdf`, which returns
zero below `1e-16`; this returns the tail itself, and the two agree to `1e-15` absolute. Under
`Debiased` the tests are F and the coefficients read Student's t with `ResidualDegreesOfFreedom`;
otherwise χ² and the normal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PanelRegression`](../panel/panelregression.md), [`PanelOptions`](paneloptions.md).
