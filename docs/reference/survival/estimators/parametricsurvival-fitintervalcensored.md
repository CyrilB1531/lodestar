# ParametricSurvival.FitIntervalCensored

Fits a parametric model to interval-censored times: lifelines' `fit_interval_censoring`.

<!-- docs-declaration -->

```csharp
public static ParametricFit FitIntervalCensored(ParametricModel model, ReadOnlySpan<double> lower, ReadOnlySpan<double> upper, ParametricOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static ParametricFit FitIntervalCensored(ParametricModel model, ReadOnlySpan<double> lower, ReadOnlySpan<double> upper, ReadOnlySpan<double> weights, ReadOnlySpan<double> entries, ParametricOptions options = null)
```

The second overload takes lifelines' `weights` and `entry`.

**Parameters** — `model` is the [`ParametricModel`](parametricmodel.md) to fit. `lower` and `upper`
bound each subject's event: `lower` non-negative, `upper` at least `lower`, infinite for a subject
still event-free at `lower`. Equal bounds are an event observed exactly. `weights` holds one positive,
finite weight per subject, or is empty for ones; `entries` each subject's entry time, non-negative, or
is empty for none. `options` sets the interval level, the iteration budget and the piecewise
breakpoints; `null` takes the defaults.

**Returns** — a [`ParametricFit`](parametricfit.md), as [`Fit`](parametricsurvival-fit.md) returns.

**Exceptions** — `ArgumentException` when the spans differ in length or are empty, a bound is `NaN`
or negative, an upper bound is below its lower one, a weight or entry span is neither empty nor one
valid value per subject, or the piecewise model is given no breakpoint.
`ArgumentOutOfRangeException` when `model` names no model. `InvalidOperationException` when the fit
does not converge, or its parameters are not identified.

**Example** — ten subjects examined at visits: one seen at the event, two still event-free at their
last visit.

```csharp
using Lodestar.Survival;

double[] lastClear = [2, 4, 0, 6, 3, 8, 5, 1, 7, 10];
double[] firstSeen = [4, 6, 3, 9, 3, double.PositiveInfinity, 7, 4, 10, double.PositiveInfinity];

ParametricFit fit = ParametricSurvival.FitIntervalCensored(ParametricModel.LogNormal, lastClear, firstSeen);

double mu = Math.Round(fit.Parameters[0], 6);        // => 1.64757
double z = Math.Round(fit.ZStatistics[1], 6);        // => -1.581959
```

**Remarks** — **a subject contributes `S(lower) − S(upper)`**, the probability the event fell between
its two visits, and an infinite `upper` makes that `S(lower)`, a right-censoring. The one design
where this is not a choice is a disease found only when looked for: dating the event at the visit
that found it biases every estimate late.

The z statistic above measures `sigma_` from one, not zero: [`ParametricFit`](parametricfit.md) has
why. Turnbull's non-parametric estimate for interval-censored data is not here.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricSurvival`](parametricsurvival.md),
[`ParametricSurvival.Fit`](parametricsurvival-fit.md),
[`AcceleratedFailureTime.FitIntervalCensored`](acceleratedfailuretime-fitintervalcensored.md).
