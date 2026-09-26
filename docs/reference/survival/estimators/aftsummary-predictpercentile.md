# AftSummary.PredictPercentile

The time by which each subject's survival falls to a level: lifelines' `predict_percentile`.

<!-- docs-declaration -->

```csharp
public double[] PredictPercentile(ReadOnlySpan<double> design, double probability)
```

**Parameters** — `design` holds the subjects' covariates row-major, `FeatureCount` per row; empty for
the one subject of a fit with no covariate. `probability` is the survival level, strictly inside
`(0, 1)`; a half for the median.

**Returns** — one time per subject.

**Exceptions** — `ArgumentException` when `design` is not whole rows of finite covariates.
`ArgumentOutOfRangeException` when `probability` is not strictly inside `(0, 1)`.

**Example** — the time by which three quarters have had the event.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

AftSummary fit = AcceleratedFailureTime.Fit(AftModel.Weibull, dose, months, died, featureCount: 1);

double[] times = fit.PredictPercentile([0.0, 2.0], 0.25);
double untreated = Math.Round(times[0], 6);   // => 22.614671
double twoUnits = Math.Round(times[1], 6);    // => 7.540571
```

**Remarks** — **the level is a survival probability**, as lifelines' is: `0.25` is the time a quarter
are still event-free, later than the median. Each model inverts its survival function in closed form.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AftSummary`](aftsummary.md), [`AftSummary.PredictMedian`](aftsummary-predictmedian.md),
[`ParametricFit.Percentile`](parametricfit-percentile.md).
