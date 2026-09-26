# AcceleratedFailureTime.FitLeftCensored

Fits an accelerated failure time regression to left-censored durations: lifelines'
`fit_left_censoring`.

<!-- docs-declaration -->

```csharp
public static AftSummary FitLeftCensored(AftModel model, ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, int featureCount, AftOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static AftSummary FitLeftCensored(AftModel model, ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> weights, ReadOnlySpan<double> entries, int featureCount, AftOptions options = null)
```

The second overload takes lifelines' `weights_col` and `entry_col`.

**Parameters** — `model`, `design`, `featureCount` and `options` are
[`Fit`](acceleratedfailuretime-fit.md)'s. `durations` holds one positive, finite duration per
subject. `eventObserved` is `true` where the duration is the event's own time, and `false` where the
event had already happened by it. `weights` holds one positive, finite weight per subject, or is
empty for ones; `entries` each subject's entry time, non-negative and at most its duration, or is
empty for none.

**Returns** — an [`AftSummary`](aftsummary.md), as [`Fit`](acceleratedfailuretime-fit.md) returns.

**Exceptions** — `ArgumentException` when the spans do not match the subjects, a value is not finite,
a duration is not positive, a weight or entry span is neither empty nor one valid value per subject,
or the primary parameter has no column at all. `ArgumentOutOfRangeException` when `model` names no
model, or `featureCount` is negative. `InvalidOperationException` when the fit does not converge, or
its parameters are not identified.

**Example** — readings under a detection limit, against a dose.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] readings = [3, 5, 2, 8, 4, 6, 1, 7, 5, 9];
bool[] exact = [true, false, true, true, false, true, false, true, true, true];

AftSummary fit = AcceleratedFailureTime.FitLeftCensored(AftModel.Weibull, dose, readings, exact, featureCount: 1);

double perUnit = Math.Round(fit.Coefficients[0], 6);          // => 0.265103
double concordance = Math.Round(fit.ConcordanceIndex, 6);     // => 0.692308
```

**Remarks** — a left-censored subject contributes `1 − S(t)` at its own linear predictors, as in
[`ParametricSurvival.FitLeftCensored`](parametricsurvival-fitleftcensored.md). The concordance index
is lifelines': the durations against the predicted medians, a duration without an observed event
counted as a censoring, as lifelines counts it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AcceleratedFailureTime`](acceleratedfailuretime.md),
[`AcceleratedFailureTime.Fit`](acceleratedfailuretime-fit.md), [`AftSummary`](aftsummary.md).
