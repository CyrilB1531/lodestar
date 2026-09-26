# AalenSummary

What an Aalen additive fit reports, at `lifelines` parity, and the predictions it makes.

<!-- docs-declaration -->

```csharp
public sealed class AalenSummary
```

**Properties** — the per-time lists are row-major, one row per event time and one column per
coefficient: the covariates in the design's order, then the intercept.

- `EventTimes` are the distinct event times the coefficients step at, ascending: lifelines' index,
  cut where the fit stopped.
- `CovariateIndices` says which design column each coefficient multiplies, `-1` for the intercept.
- `Hazards` are each coefficient's increment at each event time, lifelines' `hazards_`.
- `CumulativeHazards` are the increments summed, lifelines' `cumulative_hazards_`.
- `CumulativeVariance` is their variance, lifelines' `cumulative_variance_`.
- `ConfidenceLower` and `ConfidenceUpper` are each cumulative coefficient's normal interval at
  `ConfidenceLevel`.

For the model, one value per coefficient or one in all:

- `Slopes` are each cumulative coefficient's slope through the origin, weighted by the number at
  risk, lifelines' `summary["slope(coef)"]`, and `SlopeStandardErrors` their standard errors.
- `ConcordanceIndex` is Harrell's C, lifelines' `concordance_index_`.
- `ConfidenceLevel` is the level the intervals were built at, `FeatureCount` the number of
  covariates a design row holds, and `FitIntercept` whether the fit carries an intercept, the last
  coefficient.

**Example** — the fit of the [`AalenAdditive`](aalenadditive.md) page: the treatment, then the
intercept, over ten event times.

```csharp
using Lodestar.Survival;

double[] treated = [0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1];
double[] months = [3, 10, 5, 4, 8, 18, 12, 6, 9, 20, 7, 15];
bool[] died = [true, true, true, false, true, true, true, true, true, false, true, true];

AalenSummary fit = AalenAdditive.Fit(treated, months, died, featureCount: 1);

int times = fit.EventTimes.Count;                          // => 10
int intercept = fit.CovariateIndices[1];                   // => -1
double treatment = Math.Round(fit.CumulativeHazards[18], 6);   // => -2.083333
double lower = Math.Round(fit.ConfidenceLower[18], 6);     // => -3.851921
double slopeError = fit.SlopeStandardErrors[0];            // => 0.113183…
```

**Remarks** — **a cumulative coefficient is read by its slope, not its level.** Its value at a time
is the effect summed up to then; where it runs straight the covariate adds a constant amount to the
hazard, where it bends the effect changed. `Slopes` sums that up in one number, and
[`SmoothedHazards`](aalensummary-smoothedhazards.md) gives the slope time by time.

**lifelines' readings are kept on purpose**, so the numbers are lifelines':

- **`CumulativeVariance` is rescaled by the covariate's deviation, not its square.** The fit runs
  on covariates scaled to unit deviation, and lifelines divides the variance back by the deviation
  once, as it does the coefficients; the bounds are built on it.
- **`ConcordanceIndex` pairs the durations sorted by lifelines with the predictions in the
  design's order.** lifelines scores the training subjects that way, so the index is not Harrell's C
  of the fit unless the design came sorted by duration. With tied durations, lifelines' own pairing
  follows numpy's unstable sort, which varies with the CPU; this one sorts stably.

A class rather than a record, as [`CoxSummary`](coxsummary.md) is: it carries the fitted model its
predictions evaluate.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AalenAdditive.Fit`](aalenadditive-fit.md), [`AalenOptions`](aalenoptions.md).

## Members

| Member | What it does |
| --- | --- |
| [`AalenSummary.PredictCumulativeHazard`](aalensummary-predictcumulativehazard.md) | Each subject's cumulative hazard at every event time. |
| [`AalenSummary.PredictExpectation`](aalensummary-predictexpectation.md) | Each subject's expected lifetime, over the event times. |
| [`AalenSummary.PredictMedian`](aalensummary-predictmedian.md) | Each subject's median survival time. |
| [`AalenSummary.PredictPercentile`](aalensummary-predictpercentile.md) | The event time at which each subject's survival reaches a level. |
| [`AalenSummary.PredictSurvivalFunction`](aalensummary-predictsurvivalfunction.md) | Each subject's survival at every event time. |
| [`AalenSummary.SmoothedHazards`](aalensummary-smoothedhazards.md) | The increments smoothed by an Epanechnikov kernel. |
