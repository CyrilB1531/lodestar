# ParametricSurvival.Fit

Fits a parametric model to right-censored durations: lifelines' `fit`.

<!-- docs-declaration -->

```csharp
public static ParametricFit Fit(ParametricModel model, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ParametricOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static ParametricFit Fit(ParametricModel model, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> weights, ReadOnlySpan<double> entries, ParametricOptions options = null)
```

The second overload takes lifelines' `weights` and `entry`.

**Parameters** — `model` is the [`ParametricModel`](parametricmodel.md) to fit. `durations` holds one
positive, finite duration per subject, and `eventObserved` is `true` where it ends in the event.
`weights` holds one positive, finite weight per subject, or is empty for ones. `entries` holds each
subject's entry time, non-negative and at most its duration, or is empty when everyone is observed
from zero. `options` sets the interval level, the iteration budget and the piecewise model's
breakpoints; `null` takes the defaults.

**Returns** — a [`ParametricFit`](parametricfit.md): the parameters with their standard errors,
z statistics, p-values and intervals, the log-likelihood and AIC, and the fitted curves.

**Exceptions** — `ArgumentException` when the spans differ in length or are empty, a duration is not
positive and finite, a weight or entry span is neither empty nor one valid value per subject, or the
piecewise model is given no breakpoint. `ArgumentOutOfRangeException` when `model` names no model.
`InvalidOperationException` when the fit does not converge, or its parameters are not identified.

**Example** — a weight of two is the same subject counted twice.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];
double[] weights = [1, 2, 1, 1, 1, 2, 1, 1, 1, 1];

ParametricFit weighted = ParametricSurvival.Fit(ParametricModel.Weibull, months, died, weights, []);
ParametricFit repeated = ParametricSurvival.Fit(ParametricModel.Weibull,
    [5, 8, 12, 3, 15, 9, 20, 6, 11, 14, 8, 9],
    [true, true, false, true, true, true, false, true, true, false, true, true]);

double byWeight = Math.Round(weighted.Parameters[0], 6);   // => 12.661582
double byCopy = Math.Round(repeated.Parameters[0], 6);     // => 12.661582
```

**Remarks** — **Newton's method on exact derivatives**, from lifelines' own starting point, each step
halved until it raises the likelihood and keeps every parameter inside its bounds. The derivatives are
carried forward through the likelihood, not estimated by differences, so the observed information the
standard errors come from is the exact one at the maximum. lifelines' optimiser stops 1e-6 to 1e-3
short of that maximum: expect a default lifelines fit to differ in the sixth significant figure.

**Delayed entry conditions each subject on having survived to its entry**, lifelines' `entry`: a
subject entering at month 4 has its likelihood divided by `S(4)`, so it says nothing about the months
before it was watched.

**The piecewise exponential needs its breakpoints**, in
[`ParametricOptions.Breakpoints`](parametricoptions.md); every other model ignores them.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricSurvival`](parametricsurvival.md), [`ParametricFit`](parametricfit.md),
[`ParametricOptions`](parametricoptions.md),
[`ParametricSurvival.FitLeftCensored`](parametricsurvival-fitleftcensored.md).
