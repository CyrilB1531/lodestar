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
entity means and the rows as given. Each is `NaN` where its response does not vary, read exactly on the rows as given:
`RSquared` on a response the transform removes whole, its residuals exactly zero or not, as the model F reads `NaN` there unless they are (below), `RSquaredOverall` and `RSquaredBetween` on a constant response (zero without a
constant), the latter's sufficient case, and `RSquaredWithin` on a response constant in each entity, a constant alone
among the regressors and a panel of one period, or of one row an entity, included, where linearmodels reads 0 or the rounding
([#1779](https://github.com/CyrilB1531/lodestar/issues/1779), [#1789](https://github.com/CyrilB1531/lodestar/issues/1789)). `ModelTest` is the homoskedastic F that every coefficient but the
constant is zero, `RobustModelTest` the same under the chosen covariance (a `NaN` statistic and p-value when its block is singular, as with at most as many clusters, the coarser of two nested clusterings counted, or kernel periods, as coefficients tested, or on an exact fit, every residual exactly zero, under a robust, clustered or Driscoll-Kraay covariance, which is then zero: each standard error 0, each interval the point estimate, each t statistic and p-value `NaN`; residuals of one rounding read the ordinary formulas ([#1784](https://github.com/CyrilB1531/lodestar/issues/1784)); the unadjusted covariance stays the homoskedastic one, its test the classical F's, read as it is below but beside a constant the columns only span, where it is not the classical F and reads its ratio ([#1790](https://github.com/CyrilB1531/lodestar/issues/1790))), and `PoolabilityTest` fixed
effects' F that the effects are zero, all [`WaldTest`](../common/waldtest.md)s or `null`. The classical F is `+∞` where the
residuals are exactly zero; where they are not, a response the transform leaves nothing of reads `NaN`, read exactly
on the rows as given: constant within each entity under entity effects or random effects, whose θ it leaves one in exact
arithmetic, whatever θ the rounding computes ([#1787](https://github.com/CyrilB1531/lodestar/issues/1787)), each period under time effects, either under both, and
overall with a constant, pooled or between; linearmodels reads `−df_resid` off the rounding there, or other noise. A
response two-way effects or the entity means absorb in another way reads its ratio
([#1779](https://github.com/CyrilB1531/lodestar/issues/1779)). `PoolabilityTest` is likewise `+∞` where the fit's
residuals are exactly zero, and otherwise `NaN` on a constant response
([#1788](https://github.com/CyrilB1531/lodestar/issues/1788)).
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

The per-coefficient lists are the fit's own arrays, taken and exposed as they are, not copied: a
caller casting one back to its array and writing to it changes this summary, and every holder of
the same array ([#1305](https://github.com/CyrilB1531/lodestar/issues/1305)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`PanelRegression`](../panel/panelregression.md), [`PanelOptions`](paneloptions.md).
