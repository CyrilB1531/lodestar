# CoxSummary.PredictExpectation

The area under each subject's survival curve over the baseline's times: lifelines' `predict_expectation`.

<!-- docs-declaration -->

```csharp
public double[] PredictExpectation(ReadOnlySpan<double> design, ReadOnlySpan<int> strata)
```

**Parameters** — `design` holds the subjects' covariates row-major. `strata` holds each subject's stratum when the fit was stratified, and is empty otherwise.

**Returns** — one value per subject, by the trapezoid rule over the baseline's times.

**Exceptions** — `ArgumentException` when `design` is not a whole number of rows of finite values, or `strata` does not match the subjects or names a stratum the fit did not see, or is empty for a stratified fit.

**Example** — a subject at `(2, 1)` in the fit of the [`CoxSummary`](coxsummary.md) page.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary fit = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);
double expected = Math.Round(fit.PredictExpectation([2.0, 1.0], [])[0], 6);   // => 3.667956
```

**Remarks** — **restricted to the observed span, as lifelines computes it**: the integral runs from the first observed duration, not from zero, to the last, so it is not the unrestricted mean survival time.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxSummary`](coxsummary.md), [`CoxBaseline`](coxbaseline.md).
