# AftSummary.PredictCumulativeHazard

Each subject's cumulative hazard at given times: lifelines' `predict_cumulative_hazard`.

<!-- docs-declaration -->

```csharp
public double[] PredictCumulativeHazard(ReadOnlySpan<double> design, ReadOnlySpan<double> times)
```

**Parameters** — `design` holds the subjects' covariates row-major, `FeatureCount` per row; empty for
the one subject of a fit with no covariate. `times` are the positive, finite times to read at.

**Returns** — row-major, one row per subject and one column per time.

**Exceptions** — `ArgumentException` when `design` is not whole rows of finite covariates, or a time
is not positive and finite.

**Example** — two subjects at five and ten months.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

AftSummary fit = AcceleratedFailureTime.Fit(AftModel.Weibull, dose, months, died, featureCount: 1);

double[] hazard = fit.PredictCumulativeHazard([0.0, 2.0], [5.0, 10.0]);
double untreatedAtTen = Math.Round(hazard[1], 6);   // => 0.026655
double twoUnitsAtTen = Math.Round(hazard[3], 6);    // => 5.438795
```

**Remarks** — the first subject's row is `hazard[0]` and `hazard[1]`, the second's `hazard[2]` and
`hazard[3]`. [`PredictSurvivalFunction`](aftsummary-predictsurvivalfunction.md) is its `exp(−H)`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AftSummary`](aftsummary.md), [`AftSummary.PredictSurvivalFunction`](aftsummary-predictsurvivalfunction.md).
