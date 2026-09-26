# ParametricSurvival

lifelines' parametric univariate fitters, right-, left- and interval-censored, fitted to their
maximum.

<!-- docs-declaration -->

```csharp
public static class ParametricSurvival
```

**Example** — a Weibull curve through ten patients, three of them censored.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

ParametricFit fit = ParametricSurvival.Fit(ParametricModel.Weibull, months, died);

double scale = Math.Round(fit.Parameters[0], 6);        // => 13.682221
double shape = Math.Round(fit.Parameters[1], 6);        // => 1.702463
double median = Math.Round(fit.MedianSurvivalTime, 6);  // => 11.032146
```

**Remarks** — **a parametric fit trades the data's own shape for a smooth one.**
[`KaplanMeier`](kaplanmeier.md) steps only where an event happened and stops at the last duration;
a fitted model has a hazard at every time, a median even where fewer than half had the event, and a
curve past the end of follow-up — each as good as the model is. Compare models on the same data by
[`ParametricFit.Aic`](parametricfit.md), lower being better.

Six models, lifelines' six fitters: [`ParametricModel`](parametricmodel.md) names them and their
parameters. Every fit takes lifelines' `weights` and delayed entry (`entry`) through the overloads
that carry them, and the three censoring schemes are three methods: `Fit` for right-censored
durations, `FitLeftCensored` for durations some of which are only upper bounds, `FitIntervalCensored`
for events known to lie between two times.

**The fit is lifelines' likelihood taken to its maximum**, by Newton's method on exact first and
second derivatives. lifelines' own optimiser, Nelder-Mead then L-BFGS-B, stops 1e-6 to 1e-3 short of
that maximum, so a default lifelines fit differs from this one in about the sixth significant figure;
the frozen reference is lifelines driven to its maximum. Reference behaviour is `lifelines` 0.30.3's
`ExponentialFitter`, `WeibullFitter`, `LogNormalFitter`, `LogLogisticFitter`,
`PiecewiseExponentialFitter` and `GeneralizedGammaFitter`. Thread-safe.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the estimators index](../estimators.md), [`ParametricFit`](parametricfit.md),
[`AcceleratedFailureTime`](acceleratedfailuretime.md), [Python → C# equivalence](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`ParametricSurvival.CompareAt`](parametricsurvival-compareat.md) | Tests whether two fitted curves differ at one time. |
| [`ParametricSurvival.Fit`](parametricsurvival-fit.md) | Fits a model to right-censored durations. |
| [`ParametricSurvival.FitIntervalCensored`](parametricsurvival-fitintervalcensored.md) | Fits a model to events known to lie between two times. |
| [`ParametricSurvival.FitLeftCensored`](parametricsurvival-fitleftcensored.md) | Fits a model to durations some of which are upper bounds. |
