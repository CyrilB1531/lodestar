# ParametricSummary

A fitted parametric model: its parameters with their inference, and its curves.

<!-- docs-declaration -->

```csharp
public sealed class ParametricSummary
```

**Properties** — the per-parameter lists are parallel and in lifelines' order:

- `ParameterNames` are lifelines' names: `lambda_` and `rho_` for the Weibull, `mu_` and `sigma_` for
  the log-normal, and so on; [`ParametricModel`](parametricmodel.md) lists them.
- `Parameters` are the fitted values.
- `StandardErrors` are the square roots of the inverse observed information's diagonal.
- `ZStatistics` is each parameter less lifelines' comparison value, over its standard error.
- `PValues` are two-sided, from the normal tail.
- `ConfidenceLower` and `ConfidenceUpper` are each parameter's interval at `ConfidenceLevel`.

For the model:

- `Model` is the [`ParametricModel`](parametricmodel.md) fitted.
- `LogLikelihood` is the log-likelihood at the fit, summed over the subjects with their weights.
- `Akaike` is Akaike's criterion, `−2 · LogLikelihood + 2k` over the `k` parameters.
- `MedianSurvivalTime` is the time the fitted survival falls to one half, lifelines'
  `median_survival_time_`.
- `ConfidenceLevel` is the level the intervals were built at.

**Example** — the Weibull fit of the [`ParametricSurvival`](parametricsurvival.md) page, its shape
read against one.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

ParametricSummary fit = ParametricSurvival.Fit(months, died, ParametricModel.Weibull);

string shapeName = fit.ParameterNames[1];            // => rho_
double shapeZ = Math.Round(fit.ZStatistics[1], 6);   // => 1.306226
double shapeP = Math.Round(fit.PValues[1], 6);       // => 0.191476
double aic = Math.Round(fit.Akaike, 6);              // => 53.400484
```

**Remarks** — **each z statistic is measured from lifelines' `_compare_to_values`, not from zero.**
A Weibull `rho_` of one is the exponential model, so its z statistic above tests whether the hazard
changes with time at all, and a p-value of 0.19 says ten patients cannot tell. The values are one for
the Weibull's and the log-logistic's two parameters and for the log-normal's `sigma_`, zero for the
log-normal's `mu_`, the exponential's `lambda_` and the generalized gamma's `mu_` and `ln_sigma_`, one
for its `lambda_`, and lifelines' starting values for the piecewise exponential's rates.
[`AftSummary`](aftsummary.md) measures its coefficients from zero.

**The generalized gamma's shape derivatives are numerical on both sides.** Its likelihood holds
the regularised incomplete gamma function, differentiated in its shape parameter; lifelines'
`autograd_gamma` takes that derivative by finite differences, which puts lifelines' own standard
errors up to about 2e-5 relative off. These take central differences extrapolated by Richardson's
method. Within 0.35 of a zero `lambda_` the likelihood here follows Temme's uniform expansion about
the log-normal instead, with exact derivatives, short of the tail where `lambda_` times the log
time standardized through the expansion passes 2.2 in magnitude: through the gamma at shape
`1/lambda_²` they lost their digits, and a log-normal sample's fit did not converge. Below about 1e-10 lifelines'
own clip of that gamma's variate at `1e20` holds at every point; this follows the smooth limit there.

A class rather than a record, as [`CoxSummary`](coxsummary.md) is: it carries the model it evaluates
its curves with.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricSurvival.Fit`](parametricsurvival-fit.md),
[`ParametricModel`](parametricmodel.md), [`ParametricOptions`](parametricoptions.md).

## Members

| Member | What it does |
| --- | --- |
| [`ParametricSummary.CumulativeHazard`](parametricsummary-cumulativehazard.md) | The fitted cumulative hazard at given times. |
| [`ParametricSummary.CumulativeHazardBounds`](parametricsummary-cumulativehazardbounds.md) | The delta-method interval of the cumulative hazard. |
| [`ParametricSummary.Hazard`](parametricsummary-hazard.md) | The fitted hazard at given times. |
| [`ParametricSummary.Percentile`](parametricsummary-percentile.md) | The time by which survival falls to a level. |
| [`ParametricSummary.Survival`](parametricsummary-survival.md) | The fitted survival function at given times. |
| [`ParametricSummary.SurvivalBounds`](parametricsummary-survivalbounds.md) | The delta-method interval of the survival function. |
