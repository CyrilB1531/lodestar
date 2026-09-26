# AcceleratedFailureTime

lifelines' parametric accelerated failure time regressions, right-, left- and interval-censored,
fitted to their maximum.

<!-- docs-declaration -->

```csharp
public static class AcceleratedFailureTime
```

**Example** — how a dose stretches or shrinks time, on ten patients.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

AftSummary fit = AcceleratedFailureTime.Fit(AftModel.Weibull, dose, months, died, featureCount: 1);

double perUnit = Math.Round(fit.ExpCoefficients[0], 6);   // => 0.57744
double concordance = fit.ConcordanceIndex;                // => 0.9
```

**Remarks** — **an accelerated failure time model says a covariate multiplies time.** Above, each
unit of dose multiplies a patient's time scale by 0.577: every percentile of the survival curve comes
0.577 times as soon. [`CoxProportionalHazards`](coxproportionalhazards.md) says instead that a
covariate multiplies the hazard, and leaves the baseline's shape free; this fixes the shape to a
[`ParametricModel`](parametricmodel.md)'s and reads the effect on the time axis, where it is often
what a reader asked for. The Weibull is the one model that is both, so its coefficients and a Cox
fit's describe the same curves in two units.

The primary parameter's log (`lambda_` for the Weibull, `mu_` itself for the log-normal, `alpha_` for
the log-logistic) is linear in the covariates; the ancillary one's log is an intercept, or linear in
the same covariates when [`AftOptions.Ancillary`](aftoptions.md) says so. The design holds covariates
only: the intercept is [`AftOptions.FitIntercept`](aftoptions.md)'s to add.

**The fit is lifelines' likelihood taken to its maximum**, by Newton's method on exact derivatives,
from the univariate fit lifelines itself starts from; lifelines' optimiser stops short of it, so a
default lifelines fit differs in about the sixth significant figure. Reference behaviour is
`lifelines` 0.30.3's `WeibullAFTFitter`, `LogNormalAFTFitter` and `LogLogisticAFTFitter`, with
their weights, delayed entry, ridge penalty and robust errors. Thread-safe.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the estimators index](../estimators.md), [`AftSummary`](aftsummary.md),
[`AftOptions`](aftoptions.md), [`ParametricSurvival`](parametricsurvival.md),
[Python → C# equivalence](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`AcceleratedFailureTime.Fit`](acceleratedfailuretime-fit.md) | Fits a regression to right-censored durations. |
| [`AcceleratedFailureTime.FitIntervalCensored`](acceleratedfailuretime-fitintervalcensored.md) | Fits a regression to events known to lie between two times. |
| [`AcceleratedFailureTime.FitLeftCensored`](acceleratedfailuretime-fitleftcensored.md) | Fits a regression to durations some of which are upper bounds. |
