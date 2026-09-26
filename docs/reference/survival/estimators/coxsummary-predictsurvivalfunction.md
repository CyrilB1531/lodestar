# CoxSummary.PredictSurvivalFunction

Each subject's survival at given times: lifelines' `predict_survival_function`.

<!-- docs-declaration -->

```csharp
public double[] PredictSurvivalFunction(ReadOnlySpan<double> design, ReadOnlySpan<int> strata, ReadOnlySpan<double> times)
```

**Parameters** — `design` holds the subjects' covariates row-major. `strata` holds each subject's stratum when the fit was stratified, and is empty otherwise. `times` are the times to read at, or empty for the baseline's own.

**Returns** — row-major, one row per subject and one column per time: `exp(−H)` of the cumulative hazard.

**Exceptions** — `ArgumentException` when `design` is not a whole number of rows of finite values, or `strata` does not match the subjects or names a stratum the fit did not see, or is empty for a stratified fit, or a time is `NaN`.

**Example** — a subject at `(2, 1)` in the fit of the [`CoxSummary`](coxsummary.md) page.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary fit = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);
double[] survival = fit.PredictSurvivalFunction([2.0, 1.0], [], [5, 10]);
double atFive = Math.Round(survival[0], 6);   // => 0.497554
```

**Remarks** — the exponential of [`PredictCumulativeHazard`](coxsummary-predictcumulativehazard.md), so it interpolates the same way.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxSummary`](coxsummary.md), [`CoxBaseline`](coxbaseline.md).
