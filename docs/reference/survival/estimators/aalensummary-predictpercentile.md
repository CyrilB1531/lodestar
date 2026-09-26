# AalenSummary.PredictPercentile

The event time at which each subject's survival reaches a level: lifelines' `predict_percentile`.

<!-- docs-declaration -->

```csharp
public double[] PredictPercentile(ReadOnlySpan<double> design, double probability)
```

**Parameters** — `design` holds the subjects' covariates row-major, `FeatureCount` per row; empty
for the one subject of a fit with no covariate. `probability` is the survival level, in `[0, 1]`; a
half for the median.

**Returns** — one time per subject, among [`EventTimes`](aalensummary.md); infinity where the last
survival is still above the level.

**Exceptions** — `ArgumentException` when `design` is not whole rows of finite covariates.
`ArgumentOutOfRangeException` when `probability` lies outside `[0, 1]`.

**Example** — by when a quarter are still alive: month 9 untreated, never within the fit on
treatment.

```csharp
using Lodestar.Survival;

double[] treated = [0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1];
double[] months = [3, 10, 5, 4, 8, 18, 12, 6, 9, 20, 7, 15];
bool[] died = [true, true, true, false, true, true, true, true, true, false, true, true];

AalenSummary fit = AalenAdditive.Fit(treated, months, died, featureCount: 1);

double[] times = fit.PredictPercentile([0.0, 1.0], 0.25);
double untreated = times[0];                              // => 9
bool neverOnTreatment = double.IsPositiveInfinity(times[1]);   // => True
```

**Remarks** — **the level is a survival probability**, as lifelines' is: `0.25` is the time a
quarter are still event-free.

**The time is where numpy's `searchsorted` lands, not necessarily the first crossing.** lifelines
reads the percentile off the curve by a binary search that assumes it falls, and an additive model's
curve need not: where it rises and falls back across the level, the search may land on a later
crossing than the first. This lands where numpy's does, measured identical on 20,000 random curves.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AalenSummary`](aalensummary.md),
[`AalenSummary.PredictMedian`](aalensummary-predictmedian.md),
[`AalenSummary.PredictSurvivalFunction`](aalensummary-predictsurvivalfunction.md).
