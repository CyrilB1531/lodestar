# AalenSummary.PredictExpectation

Each subject's expected lifetime: lifelines' `predict_expectation`, the trapezoid under its
survival.

<!-- docs-declaration -->

```csharp
public double[] PredictExpectation(ReadOnlySpan<double> design)
```

**Parameters** — `design` holds the subjects' covariates row-major, `FeatureCount` per row; empty
for the one subject of a fit with no covariate.

**Returns** — one value per subject.

**Exceptions** — `ArgumentException` when `design` is not whole rows of finite covariates.

**Example** — an untreated and a treated patient.

```csharp
using Lodestar.Survival;

double[] treated = [0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1];
double[] months = [3, 10, 5, 4, 8, 18, 12, 6, 9, 20, 7, 15];
bool[] died = [true, true, true, false, true, true, true, true, true, false, true, true];

AalenSummary fit = AalenAdditive.Fit(treated, months, died, featureCount: 1);

double[] means = fit.PredictExpectation([0.0, 1.0]);
double untreated = Math.Round(means[0], 6);     // => 4.696046
double onTreatment = Math.Round(means[1], 6);   // => 11.776772
```

**Remarks** — **the trapezoid runs over [`EventTimes`](aalensummary.md), from the first event time,
not from zero**, as lifelines integrates it. The area before the first event and after the last is
left out, so the value is not a mean lifetime: above, the untreated patient's 4.67 months omits the
three before the first death, and the treated patient's stops at month 18 with nearly half still alive.
Read it as a comparison between subjects of one fit.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AalenSummary`](aalensummary.md),
[`AalenSummary.PredictMedian`](aalensummary-predictmedian.md).
