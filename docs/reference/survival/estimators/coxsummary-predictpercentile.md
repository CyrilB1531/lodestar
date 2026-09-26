# CoxSummary.PredictPercentile

The time each subject's survival first falls to a level: lifelines' `predict_percentile`.

<!-- docs-declaration -->

```csharp
public double[] PredictPercentile(ReadOnlySpan<double> design, ReadOnlySpan<int> strata, double probability)
```

**Parameters** — `design` holds the subjects' covariates row-major. `strata` holds each subject's stratum when the fit was stratified, and is empty otherwise. `probability` is the survival level, in `[0, 1]`.

**Returns** — one time per subject, among the baseline's times; infinity where the curve stays above the level.

**Exceptions** — `ArgumentException` when `design` is not a whole number of rows of finite values, or `strata` does not match the subjects or names a stratum the fit did not see, or is empty for a stratified fit. `ArgumentOutOfRangeException` when `probability` lies outside `[0, 1]`.

**Example** — a subject at `(2, 1)` in the fit of the [`CoxSummary`](coxsummary.md) page.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary fit = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);
double quartile = fit.PredictPercentile([2.0, 1.0], [], 0.25)[0];   // => 8
```

**Remarks** — the first of the baseline's times at which the survival is at or below the level, lifelines' `qth_survival_time`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxSummary`](coxsummary.md), [`CoxBaseline`](coxbaseline.md).
