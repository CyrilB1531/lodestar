# CoxSummary.PredictCumulativeHazard

Each subject's cumulative hazard at given times: lifelines' `predict_cumulative_hazard`.

<!-- docs-declaration -->

```csharp
public double[] PredictCumulativeHazard(ReadOnlySpan<double> design, ReadOnlySpan<int> strata, ReadOnlySpan<double> times)
```

**Parameters** — `design` holds the subjects' covariates row-major. `strata` holds each subject's stratum when the fit was stratified, and is empty otherwise. `times` are the times to read at, or empty for the baseline's own.

**Returns** — row-major, one row per subject and one column per time.

**Exceptions** — `ArgumentException` when `design` is not a whole number of rows of finite values, or `strata` does not match the subjects or names a stratum the fit did not see, or is empty for a stratified fit, or a time is `NaN`.

**Example** — a subject at `(2, 1)` in the fit of the [`CoxSummary`](coxsummary.md) page.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary fit = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);
double[] hazard = fit.PredictCumulativeHazard([2.0, 1.0], [], [5, 10]);
double atTen = Math.Round(hazard[1], 6);   // => 2.64375
```

**Remarks** — the baseline's cumulative hazard times the subject's partial hazard. **Between the baseline's times it is interpolated linearly**, and held flat past either end, as lifelines reads it through `numpy.interp`; at the baseline's own times it is the step.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxSummary`](coxsummary.md), [`CoxBaseline`](coxbaseline.md).
