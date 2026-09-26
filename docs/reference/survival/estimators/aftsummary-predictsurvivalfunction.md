# AftSummary.PredictSurvivalFunction

Each subject's survival at given times: lifelines' `predict_survival_function`.

<!-- docs-declaration -->

```csharp
public double[] PredictSurvivalFunction(ReadOnlySpan<double> design, ReadOnlySpan<double> times)
```

**Parameters** — `design` holds the subjects' covariates row-major, `FeatureCount` per row; empty for
the one subject of a fit with no covariate. `times` are the positive, finite times to read at.

**Returns** — row-major, one row per subject and one column per time: `exp(−H)` of
[`PredictCumulativeHazard`](aftsummary-predictcumulativehazard.md).

**Exceptions** — `ArgumentException` when `design` is not whole rows of finite covariates, or a time
is not positive and finite.

**Example** — two subjects at ten months.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

AftSummary fit = AcceleratedFailureTime.Fit(AftModel.Weibull, dose, months, died, featureCount: 1);

double[] survival = fit.PredictSurvivalFunction([0.0, 2.0], [10.0]);
double untreated = Math.Round(survival[0], 6);   // => 0.973697
double twoUnits = Math.Round(survival[1], 6);    // => 0.004345
```

**Remarks** — an empty `times` reads at no time and returns an empty array: a parametric curve has
no event times of its own to default to, where [`CoxSummary.PredictSurvivalFunction`](coxsummary-predictsurvivalfunction.md)
falls back on its baseline's.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AftSummary`](aftsummary.md), [`AftSummary.PredictCumulativeHazard`](aftsummary-predictcumulativehazard.md).
