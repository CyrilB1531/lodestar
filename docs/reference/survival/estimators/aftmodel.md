# AftModel

Which of lifelines' accelerated failure time regressions
[`AcceleratedFailureTime`](acceleratedfailuretime.md) fits.

<!-- docs-declaration -->

```csharp
public enum AftModel { Weibull, LogNormal, LogLogistic }
```

**Members** — each with its primary and ancillary parameter, as
[`AftSummary.ParameterNames`](aftsummary.md) names them:

- `Weibull`, `log λ = Xβ` and `log ρ` an intercept or `Xγ`: `lambda_`, `rho_`. lifelines'
  `WeibullAFTFitter`.
- `LogNormal`, `μ = Xβ` and `log σ` an intercept or `Xγ`: `mu_`, `sigma_`. `LogNormalAFTFitter`.
- `LogLogistic`, `log α = Xβ` and `log β` an intercept or `Xγ`: `alpha_`, `beta_`.
  `LogLogisticAFTFitter`.

**Example** — the dose's coefficient under each model: the time ratio barely depends on the shape
assumed.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

double weibull = Math.Round(AcceleratedFailureTime.Fit(AftModel.Weibull, dose, months, died, 1).Coefficients[0], 4);           // => -0.5492
double logNormal = Math.Round(AcceleratedFailureTime.Fit(AftModel.LogNormal, dose, months, died, 1).Coefficients[0], 4);       // => -0.5588
double logLogistic = Math.Round(AcceleratedFailureTime.Fit(AftModel.LogLogistic, dose, months, died, 1).Coefficients[0], 4);   // => -0.5525
```

**Remarks** — in all three **the primary coefficient is a log time ratio**: `log T` is linear in the
covariates plus an error whose law the model names — extreme-value for the Weibull, normal for the
log-normal, logistic for the log-logistic. The ancillary parameter sets that error's spread.

The Weibull is also a proportional hazards model, so its fit can be read beside a
[`CoxProportionalHazards`](coxproportionalhazards.md) one; the log-logistic is a proportional odds
model, and the log-normal neither.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AcceleratedFailureTime`](acceleratedfailuretime.md), [`ParametricModel`](parametricmodel.md).
