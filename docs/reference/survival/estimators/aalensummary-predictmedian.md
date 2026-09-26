# AalenSummary.PredictMedian

Each subject's median survival time: lifelines' `predict_median`.

<!-- docs-declaration -->

```csharp
public double[] PredictMedian(ReadOnlySpan<double> design)
```

**Parameters** — `design` holds the subjects' covariates row-major, `FeatureCount` per row; empty
for the one subject of a fit with no covariate.

**Returns** — one time per subject, among [`EventTimes`](aalensummary.md); infinity where the last
survival is still above one half.

**Exceptions** — `ArgumentException` when `design` is not whole rows of finite covariates.

**Example** — an untreated patient, and a treated one whose survival never falls to one half.

```csharp
using Lodestar.Survival;

double[] treated = [0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1];
double[] months = [3, 10, 5, 4, 8, 18, 12, 6, 9, 20, 7, 15];
bool[] died = [true, true, true, false, true, true, true, true, true, false, true, true];

AalenSummary fit = AalenAdditive.Fit(treated, months, died, featureCount: 1);

double[] medians = fit.PredictMedian([0.0, 1.0]);
double untreated = medians[0];     // => 8
bool neverOnTreatment = double.IsPositiveInfinity(medians[1]);   // => True
```

**Remarks** — [`PredictPercentile`](aalensummary-predictpercentile.md) at one half, with the same
reading of a curve that does not fall monotonically.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AalenSummary`](aalensummary.md),
[`AalenSummary.PredictExpectation`](aalensummary-predictexpectation.md).
