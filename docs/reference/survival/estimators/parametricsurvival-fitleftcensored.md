# ParametricSurvival.FitLeftCensored

Fits a parametric model to left-censored durations: lifelines' `fit_left_censoring`.

<!-- docs-declaration -->

```csharp
public static ParametricFit FitLeftCensored(ParametricModel model, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ParametricOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static ParametricFit FitLeftCensored(ParametricModel model, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> weights, ReadOnlySpan<double> entries, ParametricOptions options = null)
```

The second overload takes lifelines' `weights` and `entry`.

**Parameters** — `model` is the [`ParametricModel`](parametricmodel.md) to fit. `durations` holds one
positive, finite duration per subject. `eventObserved` is `true` where the duration is the event's own
time, and `false` where the event had already happened by it. `weights` holds one positive, finite
weight per subject, or is empty for ones; `entries` each subject's entry time, non-negative, or is
empty for none. `options` sets the interval level, the iteration budget and the piecewise
breakpoints; `null` takes the defaults.

**Returns** — a [`ParametricFit`](parametricfit.md), as [`Fit`](parametricsurvival-fit.md) returns.

**Exceptions** — `ArgumentException` when the spans differ in length or are empty, a duration is not
positive and finite, a weight or entry span is neither empty nor one valid value per subject, or the
piecewise model is given no breakpoint. `ArgumentOutOfRangeException` when `model` names no model.
`InvalidOperationException` when the fit does not converge, or its parameters are not identified.

**Example** — ten samples under a detection limit: three were only known to be positive by their
reading.

```csharp
using Lodestar.Survival;

double[] readings = [3, 5, 2, 8, 4, 6, 1, 7, 5, 9];
bool[] exact = [true, false, true, true, false, true, false, true, true, true];

ParametricFit fit = ParametricSurvival.FitLeftCensored(ParametricModel.Weibull, readings, exact);

double scale = Math.Round(fit.Parameters[0], 6);        // => 5.050534
double median = Math.Round(fit.MedianSurvivalTime, 6);  // => 4.014061
```

**Remarks** — **a left-censored subject contributes `1 − S(t)`**, the probability the event came by
`t`, where a right-censored one contributes `S(t)`. The typical case is a measurement below a
detection limit, or a subject who already had the event when first examined. Every other part of the
fit is [`Fit`](parametricsurvival-fit.md)'s, the maximum reached by Newton's method rather than where
lifelines' optimiser stops.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricSurvival`](parametricsurvival.md),
[`KaplanMeier.EstimateLeftCensored`](kaplanmeier-estimateleftcensored.md),
[`ParametricSurvival.FitIntervalCensored`](parametricsurvival-fitintervalcensored.md).
