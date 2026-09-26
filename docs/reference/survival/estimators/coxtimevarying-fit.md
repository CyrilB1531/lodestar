# CoxTimeVarying.Fit

Fits the Cox model on start-stop intervals.

<!-- docs-declaration -->

```csharp
public static CoxSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> starts, ReadOnlySpan<double> stops, ReadOnlySpan<bool> eventObserved, int featureCount, CoxOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static CoxSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> starts, ReadOnlySpan<double> stops, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> weights, ReadOnlySpan<int> strata, int featureCount, CoxOptions options = null)
```

The second overload weighs and stratifies the intervals, lifelines' `weights_col` and `strata`.

**Parameters** — `design` holds the covariates row-major, `featureCount` values per interval.
`starts` and `stops` bound each interval `(start, stop]`, starts non-negative and each stop after its
start. `eventObserved` is `true` where the interval ends in the subject's event. `weights` holds one
positive, finite weight per interval and `strata` one label per interval, or either is empty.
`options` sets the level, the iteration budget and the penalty; `null` takes the defaults.

**Returns** — a [`CoxSummary`](coxsummary.md): the coefficient table, the likelihood-ratio test and one
baseline. `ConcordanceIndex` is `NaN`, as lifelines computes none for this fitter.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is below one. `ArgumentException`
when the spans disagree in length, an interval is not finite and non-negative with its stop after its
start, a covariate is not finite or does not vary, no interval ends in an event, a covariate is
collinear with the others or separates the events, or `CoxOptions.Robust` is asked for.
`InvalidOperationException` when the fit does not converge.

**Example** — one interval per subject from zero is the ordinary fit.

```csharp
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];
double[] zeros = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

double ordinary = CoxProportionalHazards.Fit(design, months, died, featureCount: 2).Coefficients[0];
double varying = CoxTimeVarying.Fit(design, zeros, months, died, featureCount: 2).Coefficients[0];

double same = Math.Round(varying - ordinary, 9);   // => 0
```

**Remarks** — **no robust variance**: lifelines' time-varying fitter raises `NotImplementedError` for
`robust=True`, so there is nothing to match and it is refused. The penalty is the proportional
hazards fit's; with an L1 part the answer is lifelines' own loop, which sharpens its smoothed absolute
value by `1.5^i` here rather than `1.3^i`. **The baseline is lifelines'**: Breslow's cumulative hazard
at every distinct event time, pooled over the strata, the weighted events over the unweighted partial
hazards of the intervals at risk.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxTimeVarying`](coxtimevarying.md), [`CoxOptions`](coxoptions.md).
