# AftSummary.PredictExpectation

Each subject's expected survival time: lifelines' `predict_expectation`, in the model's closed form.

<!-- docs-declaration -->

```csharp
public double[] PredictExpectation(ReadOnlySpan<double> design)
```

**Parameters** — `design` holds the subjects' covariates row-major, `FeatureCount` per row; empty for
the one subject of a fit with no covariate.

**Returns** — one time per subject; `NaN` for a log-logistic whose shape is at most one, where the
mean diverges.

**Exceptions** — `ArgumentException` when `design` is not whole rows of finite covariates.

**Example** — the mean lies below the median here, the Weibull's shape being well above one.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

AftSummary fit = AcceleratedFailureTime.Fit(AftModel.Weibull, dose, months, died, featureCount: 1);

double[] means = fit.PredictExpectation([0.0, 2.0]);
double untreated = Math.Round(means[0], 6);   // => 19.37367
double twoUnits = Math.Round(means[1], 6);    // => 6.459901
```

**Remarks** — **the mean is the model's, over all time**: `λ Γ(1 + 1/ρ)` for the Weibull,
`exp(μ + σ²/2)` for the log-normal, `α π/β / sin(π/β)` for the log-logistic. It depends on the
right tail, which the data may never have reached; the median does not.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AftSummary`](aftsummary.md), [`AftSummary.PredictMedian`](aftsummary-predictmedian.md).
