# AcceleratedFailureTime.FitIntervalCensored

Fits an accelerated failure time regression to interval-censored times: lifelines'
`fit_interval_censoring`.

<!-- docs-declaration -->

```csharp
public static AftSummary FitIntervalCensored(AftModel model, ReadOnlySpan<double> design, ReadOnlySpan<double> lower, ReadOnlySpan<double> upper, int featureCount, AftOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static AftSummary FitIntervalCensored(AftModel model, ReadOnlySpan<double> design, ReadOnlySpan<double> lower, ReadOnlySpan<double> upper, ReadOnlySpan<double> weights, ReadOnlySpan<double> entries, int featureCount, AftOptions options = null)
```

The second overload takes lifelines' `weights_col` and `entry_col`.

**Parameters** — `model`, `design`, `featureCount` and `options` are
[`Fit`](acceleratedfailuretime-fit.md)'s. `lower` and `upper` bound each subject's event: `lower`
non-negative, `upper` at least `lower`, infinite for a subject still event-free at `lower`; equal
bounds are an event observed exactly. `weights` holds one positive, finite weight per subject, or is
empty for ones; `entries` each subject's entry time, non-negative and at most its upper bound, or is
empty for none.

**Returns** — an [`AftSummary`](aftsummary.md), as [`Fit`](acceleratedfailuretime-fit.md) returns,
with a `ConcordanceIndex` of `NaN`.

**Exceptions** — `ArgumentException` when the spans do not match the subjects, a covariate is not
finite, a bound is `NaN` or negative, an upper bound is below its lower one, a weight or entry span is
neither empty nor one valid value per subject, or the primary parameter has no column at all.
`ArgumentOutOfRangeException` when `model` names no model, or `featureCount` is negative.
`InvalidOperationException` when the fit does not converge, or its parameters are not identified.

**Example** — visits bracketing each event, against a dose.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] lastClear = [2, 4, 0, 6, 3, 8, 5, 1, 7, 10];
double[] firstSeen = [4, 6, 3, 9, 3, double.PositiveInfinity, 7, 4, 10, double.PositiveInfinity];

AftSummary fit = AcceleratedFailureTime.FitIntervalCensored(AftModel.Weibull, dose, lastClear, firstSeen, featureCount: 1);

double perUnit = Math.Round(fit.Coefficients[0], 6);   // => -0.017618
bool unscored = double.IsNaN(fit.ConcordanceIndex);    // => True
```

**Remarks** — **there is no concordance index for an interval-censored fit.** lifelines computes
none, and reading its `concordance_index_` raises `AttributeError`; `NaN` says the same without a
throw. A subject contributes `S(lower) − S(upper)` at its own linear predictors, as in
[`ParametricSurvival.FitIntervalCensored`](parametricsurvival-fitintervalcensored.md).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AcceleratedFailureTime`](acceleratedfailuretime.md),
[`AcceleratedFailureTime.Fit`](acceleratedfailuretime-fit.md), [`AftSummary`](aftsummary.md).
