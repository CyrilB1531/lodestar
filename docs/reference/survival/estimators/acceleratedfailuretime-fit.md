# AcceleratedFailureTime.Fit

Fits an accelerated failure time regression to right-censored durations: lifelines' `fit`.

<!-- docs-declaration -->

```csharp
public static AftSummary Fit(AftModel model, ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, int featureCount, AftOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static AftSummary Fit(AftModel model, ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> weights, ReadOnlySpan<double> entries, int featureCount, AftOptions options = null)
```

The second overload takes lifelines' `weights_col` and `entry_col`.

**Parameters** — `model` is the [`AftModel`](aftmodel.md) to fit. `design` holds the covariates
row-major, `featureCount` values per subject, with no intercept column. `durations` holds one
positive, finite duration per subject, and `eventObserved` is `true` where it ends in the event.
`weights` holds one positive, finite weight per subject, or is empty for ones; `entries` each
subject's entry time, non-negative and at most its duration, or is empty for none. `featureCount` is
the design's row length, zero for a fit with no covariate. `options` sets the level, the intercept,
the ancillary model, the penalty and the robust errors; `null` takes the defaults.

**Returns** — an [`AftSummary`](aftsummary.md): per coefficient its estimate, standard error,
z statistic, p-value and interval, with their exponentials; for the model the log-likelihood, the
likelihood-ratio test against the univariate fit, the AIC and the concordance index.

**Exceptions** — `ArgumentException` when the spans do not match the subjects, a value is not finite,
a duration is not positive, a weight or entry span is neither empty nor one valid value per subject,
or the primary parameter has no column at all. `ArgumentOutOfRangeException` when `model` names no
model, or `featureCount` is negative. `InvalidOperationException` when the fit does not converge, or
its parameters are not identified.

**Example** — a design that is not a whole number of rows is refused.

```csharp
using Lodestar.Survival;

double[] dose = [1.0, 2.0, 0.5, 3.0, 0.0, 1.5, 0.0, 2.5, 1.0, 0.5];
double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

string refusal;
try
{
    // Ten values read as rows of two are five subjects, not the ten durations.
    AcceleratedFailureTime.Fit(AftModel.Weibull, dose, months, died, featureCount: 2);
    refusal = "fitted";
}
catch (ArgumentException error)
{
    refusal = error.ParamName!;
}

string parameter = refusal;  // => design
```

**Remarks** — **the coefficients come in lifelines' order**: the primary parameter's covariates, then
its intercept, then the ancillary block — its intercept alone by default, or its covariates and then
its intercept under [`AftOptions.Ancillary`](aftoptions.md).
[`AftSummary.ParameterNames`](aftsummary.md) and `CovariateIndices` say which is which.

**Newton's method on exact derivatives**, from the univariate fit lifelines starts from, each
parameter the intercept of its block; that univariate fit is also the null model of the
likelihood-ratio test. lifelines' own optimiser stops short of the maximum this reaches, so a default
lifelines fit differs in about the sixth significant figure.

**A design that does not identify the coefficients is refused** — a covariate that does not vary is
collinear with the intercept, and the observed information is then singular.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AcceleratedFailureTime`](acceleratedfailuretime.md), [`AftSummary`](aftsummary.md),
[`AftOptions`](aftoptions.md).
