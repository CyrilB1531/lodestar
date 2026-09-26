# AalenSummary.PredictCumulativeHazard

Each subject's cumulative hazard at every event time: lifelines' `predict_cumulative_hazard`.

<!-- docs-declaration -->

```csharp
public double[] PredictCumulativeHazard(ReadOnlySpan<double> design)
```

**Parameters** — `design` holds the subjects' covariates row-major, `FeatureCount` per row; empty
for the one subject of a fit with no covariate.

**Returns** — row-major, one row per subject and one column per
[`EventTimes`](aalensummary.md) entry: each subject's covariates, with a one for the intercept,
times the cumulative coefficients.

**Exceptions** — `ArgumentException` when `design` is not whole rows of finite covariates.

**Example** — an untreated and a treated patient, at the last of the ten event times.

```csharp
using Lodestar.Survival;

double[] treated = [0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1];
double[] months = [3, 10, 5, 4, 8, 18, 12, 6, 9, 20, 7, 15];
bool[] died = [true, true, true, false, true, true, true, true, true, false, true, true];

AalenSummary fit = AalenAdditive.Fit(treated, months, died, featureCount: 1);

double[] hazard = fit.PredictCumulativeHazard([0.0, 1.0]);
double untreated = Math.Round(hazard[9], 6);   // => 2.45
double onTreatment = Math.Round(hazard[19], 6);   // => 0.366667
```

**Remarks** — **the prediction is linear in the covariates**, which is what "additive" means: the
treated patient's hazard is the untreated one's plus the treatment's cumulative coefficient,
`2.45 − 2.083333`. Nothing keeps it non-negative or non-decreasing; a design far from the fitted one can
give a cumulative hazard that falls.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AalenSummary`](aalensummary.md),
[`AalenSummary.PredictSurvivalFunction`](aalensummary-predictsurvivalfunction.md).
