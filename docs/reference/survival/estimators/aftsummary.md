# AftSummary

What an accelerated failure time fit reports, at `lifelines` parity, and the predictions it makes.

<!-- docs-declaration -->

```csharp
public sealed class AftSummary
```

**Properties** — the per-coefficient lists are parallel and in lifelines' order: the primary
parameter's covariates, then its intercept, then the ancillary parameter's block.

- `ParameterNames` says which parameter each coefficient belongs to: `lambda_` and `rho_` for the
  Weibull, `mu_` and `sigma_` for the log-normal, `alpha_` and `beta_` for the log-logistic.
- `CovariateIndices` says which design column it multiplies, `-1` for an intercept.
- `Coefficients` are the estimates: the log of the parameter is the sum of each times its covariate.
- `StandardErrors` are the square roots of the covariance's diagonal: the inverse observed
  information, or the sandwich when `Robust`.
- `ZStatistics` is each coefficient over its standard error, and `PValues` are two-sided, from the
  normal tail.
- `ConfidenceLower` and `ConfidenceUpper` are each coefficient's interval at `ConfidenceLevel`.
- `ExpCoefficients`, `ExpConfidenceLower` and `ExpConfidenceUpper` are the exponentials of the three
  above: how much a unit of the covariate multiplies the parameter.

For the model:

- `Model` is the [`AftModel`](aftmodel.md) fitted, and `FeatureCount` the number of covariates a
  design row holds.
- `LogLikelihood` is the log-likelihood at the fit, less the penalty on its summed scale: lifelines'
  `log_likelihood_`. `NullLogLikelihood` is the univariate model's, with no covariate at all.
- `LikelihoodRatioStatistic` is twice their difference, and `LikelihoodRatioPValue` its chi-squared
  tail on `LikelihoodRatioDegreesOfFreedom` degrees, the coefficients past the univariate model's two;
  `NaN` when there are none.
- `Aic` is `−2 · LogLikelihood + 2k` over the `k` coefficients.
- `ConcordanceIndex` is Harrell's C between the durations and the predicted medians; `NaN` for an
  interval-censored fit.
- `ConfidenceLevel` is the level the intervals were built at, and `Robust` says whether the standard
  errors, and everything built on them, are the Huber sandwich.

**Example** — the fit of the [`AcceleratedFailureTime`](acceleratedfailuretime.md) page: the dose,
the Weibull scale's intercept, and its shape.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

AftSummary fit = AcceleratedFailureTime.Fit(AftModel.Weibull, dose, months, died, featureCount: 1);

string intercept = fit.ParameterNames[1];              // => lambda_
int column = fit.CovariateIndices[1];                  // => -1
string shape = fit.ParameterNames[2];                  // => rho_
double statistic = Math.Round(fit.LikelihoodRatioStatistic, 6);   // => 15.822317
double modelP = fit.LikelihoodRatioPValue;             // => 6.9577…
int degrees = fit.LikelihoodRatioDegreesOfFreedom;     // => 1
```

**Remarks** — **every z statistic is measured from zero**, a coefficient that does not move its
parameter. [`ParametricFit`](parametricfit.md)'s are measured from lifelines' comparison values
instead, because its parameters are not on the log scale.

**An exponentiated coefficient multiplies the parameter, not the hazard.** For the Weibull, `lambda_`
is the time scale, so 0.577 per unit of dose means every percentile comes 0.577 times as soon; a Cox
hazard ratio would say how much more often the event happens. The two agree only through the shape:
for the Weibull, a hazard ratio is the time ratio to the power `−ρ`.

A class rather than a record, as [`CoxSummary`](coxsummary.md) is: it carries the fitted model its
predictions evaluate.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AcceleratedFailureTime.Fit`](acceleratedfailuretime-fit.md), [`AftOptions`](aftoptions.md).

## Members

| Member | What it does |
| --- | --- |
| [`AftSummary.PredictCumulativeHazard`](aftsummary-predictcumulativehazard.md) | Each subject's cumulative hazard at given times. |
| [`AftSummary.PredictExpectation`](aftsummary-predictexpectation.md) | Each subject's expected survival time. |
| [`AftSummary.PredictMedian`](aftsummary-predictmedian.md) | Each subject's median survival time. |
| [`AftSummary.PredictPercentile`](aftsummary-predictpercentile.md) | The time each subject's survival falls to a level. |
| [`AftSummary.PredictSurvivalFunction`](aftsummary-predictsurvivalfunction.md) | Each subject's survival at given times. |
