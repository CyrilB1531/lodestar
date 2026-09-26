# CoxSummary.PredictMedian

Each subject's median survival time: lifelines' `predict_median`.

<!-- docs-declaration -->

```csharp
public double[] PredictMedian(ReadOnlySpan<double> design, ReadOnlySpan<int> strata)
```

**Parameters** — `design` holds the subjects' covariates row-major. `strata` holds each subject's stratum when the fit was stratified, and is empty otherwise.

**Returns** — one time per subject, infinity where the curve stays above one half.

**Exceptions** — `ArgumentException` when `design` is not a whole number of rows of finite values, or `strata` does not match the subjects or names a stratum the fit did not see, or is empty for a stratified fit.

**Example** — a subject at `(2, 1)` in the fit of the [`CoxSummary`](coxsummary.md) page.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary fit = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);
double median = fit.PredictMedian([2.0, 1.0], [])[0];   // => 5
```

**Remarks** — [`PredictPercentile`](coxsummary-predictpercentile.md) at one half.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxSummary`](coxsummary.md), [`CoxBaseline`](coxbaseline.md).
