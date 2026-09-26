# AftOptions

What an [`AcceleratedFailureTime`](acceleratedfailuretime.md) fit may be told.

<!-- docs-declaration -->

```csharp
public sealed record AftOptions
```

**Properties** — `ConfidenceLevel` is the two-sided level the intervals are reported at; `0.95` by
default. `MaximumIterations` is how many Newton steps the fit may take; `100` by default.
`FitIntercept` is lifelines' `fit_intercept`: whether the primary parameter's linear predictor
carries an intercept, `true` by default. `Ancillary` is lifelines' `ancillary=True`: whether the
ancillary parameter is modelled by the same covariates, `false` by default, an intercept alone.
`Penalizer` is lifelines' `penalizer` at `l1_ratio=0`, a ridge penalty's weight, zero by default.
`Robust` asks for the Huber sandwich standard errors, lifelines' `robust=True`, `false` by default.

**Exceptions** — `ArgumentOutOfRangeException` when `ConfidenceLevel` does not lie strictly inside
`(0, 1)`, when `MaximumIterations` is below one, or when `Penalizer` is negative, infinite or `NaN`.
Each is thrown where the setting is set, not where the fit reads it.

**Example** — the dose also moving the Weibull's shape: one more coefficient, on `rho_`.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

AftSummary fit = AcceleratedFailureTime.Fit(AftModel.Weibull, dose, months, died, 1,
    new AftOptions { Ancillary = true });

int coefficients = fit.Coefficients.Count;                  // => 4
string third = fit.ParameterNames[2];                       // => rho_
int thirdColumn = fit.CovariateIndices[2];                  // => 0
double onShape = Math.Round(fit.Coefficients[2], 6);        // => 0.173917
```

**Remarks** — **`Ancillary` works for all three models.** lifelines' `LogLogisticAFTFitter`
constructor does not pass its `model_ancillary` on, so the log-logistic's `beta_` is an intercept
there whatever is asked; here the setting is read for every [`AftModel`](aftmodel.md).

**The penalty is the ridge part of lifelines' elastic net**, `penalizer · Σ β² / 2` on the
coefficients of the covariates scaled by their sample deviations, added to the mean negative
log-likelihood; the L1 part is not written, for the reason [`docs/equivalence.md`](../../../equivalence.md) gives. **An intercept is left unpenalised only when its block
holds covariates**, as lifelines leaves it: by default the ancillary block is an intercept alone, so
its `rho_` intercept is penalised and shrinks towards zero, a Weibull shape towards one.
`LogLikelihood` is then the penalised one, as lifelines' `log_likelihood_` is.

**`Robust` is lifelines' Huber sandwich**: the inverse information either side of the summed outer
products of each subject's score, its penalty gradient included. It changes the standard errors and
everything built on them, never the coefficients.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AcceleratedFailureTime.Fit`](acceleratedfailuretime-fit.md), [`AftSummary`](aftsummary.md).
