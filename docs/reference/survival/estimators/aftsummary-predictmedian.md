# AftSummary.PredictMedian

Each subject's median survival time: lifelines' `predict_median`.

<!-- docs-declaration -->

```csharp
public double[] PredictMedian(ReadOnlySpan<double> design)
```

**Parameters** — `design` holds the subjects' covariates row-major, `FeatureCount` per row; empty for
the one subject of a fit with no covariate.

**Returns** — one time per subject.

**Exceptions** — `ArgumentException` when `design` is not whole rows of finite covariates.

**Example** — no dose, and two units of it, in the fit of the [`AftSummary`](aftsummary.md) page.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

AftSummary fit = AcceleratedFailureTime.Fit(AftModel.Weibull, dose, months, died, featureCount: 1);

double[] medians = fit.PredictMedian([0.0, 2.0]);
double untreated = Math.Round(medians[0], 6);   // => 19.598539
double twoUnits = Math.Round(medians[1], 6);    // => 6.534881
```

**Remarks** — [`PredictPercentile`](aftsummary-predictpercentile.md) at one half. The ratio of the
two medians above, 0.333, is `ExpCoefficients[0]` squared: under an accelerated failure time model
every percentile moves by the same factor.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AftSummary`](aftsummary.md), [`AftSummary.PredictExpectation`](aftsummary-predictexpectation.md).
