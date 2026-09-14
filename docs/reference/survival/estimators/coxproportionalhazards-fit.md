# CoxProportionalHazards.Fit

Fits the model and reports its inference table.

<!-- docs-declaration -->

```csharp
public static CoxSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, int featureCount, CoxOptions options = null)
```

**Parameters** — `design` holds the covariates row-major, `featureCount` values per subject, in the
subjects' order. `durations` and `eventObserved` are the pair [`KaplanMeier.Estimate`](kaplanmeier-estimate.md)
takes: one non-negative duration per subject, and `true` where it ends in the event. `featureCount`
is how many covariates each subject carries, at least one. `options` sets the interval level and the
iteration budget; `null` takes the defaults.

**Returns** — a [`CoxSummary`](coxsummary.md): per covariate the coefficient, its standard error,
z statistic, p-value, interval and hazard ratio; for the model the log and null log partial
likelihood, the likelihood-ratio test and the concordance index.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is below one.
`ArgumentException` in any of these cases:

- the spans disagree in length, or a duration is negative or `NaN`;
- a covariate is not finite;
- no event is observed;
- a covariate separates the events, or is collinear with the others.

`InvalidOperationException` when the fit does not converge within `CoxOptions.MaximumIterations`.

**Example** — a covariate the others already determine is refused rather than fitted.

```csharp
using Lodestar.Survival;

double[] months = [5, 3, 8, 3, 9, 6];
bool[] died = [true, true, false, true, true, true];

// The second column repeats the first, so no coefficient can be told from the other.
double[] duplicated = [0.2, 0.2, 1.1, 1.1, -0.4, -0.4, 0.9, 0.9, -1.3, -1.3, 0.5, 0.5];

string refusal;
try
{
    CoxProportionalHazards.Fit(duplicated, months, died, featureCount: 2);
    refusal = "fitted";
}
catch (ArgumentException error)
{
    refusal = error.ParamName!;
}

string parameter = refusal;  // => design
```

**Remarks** — **Newton-Raphson from zero, undamped**, stopping when the largest step component falls
below `1e-10`. Four or five iterations reach it on the reference's fixtures. A fit that exhausts its
budget throws: a table built from where it stopped would carry standard errors of no meaning.

**Ties are handled by Efron's method**, the only one the reference offers. At a time carrying `m`
events, the `l`-th divides by the risk set less `l/m` of the tied events' own share. With no tie that
is the same as Breslow's.

**Two designs are refused where the reference returns numbers behind a warning.**

- **Collinear**: a covariate the others determine makes the observed information singular, and
  its Cholesky factor is refused.
- **Separated**: a covariate that orders every event before every later subject has no maximum.
  Its likelihood rises forever, and in floating point the fit stops once the score underflows to
  zero, at a large coefficient with an enormous standard error. That case is detected by the
  information collapsing to a vanishing fraction of its value at zero.

**The frozen reference is lifelines fitted to its maximum**, at `precision=1e-20`. At its defaults it
stops up to 8.6e-6 relative short of it, so a caller comparing against a default `CoxPHFitter` should
expect the coefficients to differ in the sixth significant figure.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxProportionalHazards`](coxproportionalhazards.md), [`CoxSummary`](coxsummary.md),
[`CoxOptions`](coxoptions.md).
