# CoxSummary.PredictPartialHazard

Each subject's partial hazard: lifelines' `predict_partial_hazard`.

<!-- docs-declaration -->

```csharp
public double[] PredictPartialHazard(ReadOnlySpan<double> design)
```

**Parameters** — `design` holds the subjects' covariates row-major, one value per coefficient per subject.

**Returns** — one value per subject, the exponential of its log partial hazard.

**Exceptions** — `ArgumentException` when `design` is not a whole number of rows of finite values.

**Example** — a subject at `(2, 1)` in the fit of the [`CoxSummary`](coxsummary.md) page.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary fit = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);
double value = Math.Round(fit.PredictPartialHazard([2.0, 1.0])[0], 6);   // => 2.942432
```

**Remarks** — how many times the hazard of a subject at the covariates' means this subject's is.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxSummary`](coxsummary.md), [`CoxBaseline`](coxbaseline.md).
