# AalenSummary.PredictSurvivalFunction

Each subject's survival at every event time: lifelines' `predict_survival_function`.

<!-- docs-declaration -->

```csharp
public double[] PredictSurvivalFunction(ReadOnlySpan<double> design)
```

**Parameters** — `design` holds the subjects' covariates row-major, `FeatureCount` per row; empty
for the one subject of a fit with no covariate.

**Returns** — row-major, one row per subject and one column per
[`EventTimes`](aalensummary.md) entry: `exp(−H)` of the cumulative hazard.

**Exceptions** — `ArgumentException` when `design` is not whole rows of finite covariates.

**Example** — an untreated and a treated patient, at the last of the ten event times.

```csharp
using Lodestar.Survival;

double[] treated = [0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1];
double[] months = [3, 10, 5, 4, 8, 18, 12, 6, 9, 20, 7, 15];
bool[] died = [true, true, true, false, true, true, true, true, true, false, true, true];

AalenSummary fit = AalenAdditive.Fit(treated, months, died, featureCount: 1);

double[] survival = fit.PredictSurvivalFunction([0.0, 1.0]);
double untreated = Math.Round(survival[9], 6);   // => 0.086294
double onTreatment = Math.Round(survival[19], 6);   // => 0.693041
```

**Remarks** — the exponential of
[`PredictCumulativeHazard`](aalensummary-predictcumulativehazard.md), read only at the event times.
**The curve need not fall monotonically**: an increment may be negative, and survival then rises,
past one where the cumulative hazard goes below zero.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AalenSummary`](aalensummary.md),
[`AalenSummary.PredictPercentile`](aalensummary-predictpercentile.md).
