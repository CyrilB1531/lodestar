# CoxProportionalHazards.TestProportionalHazards

Tests each covariate for a hazard ratio that drifts with time: lifelines' `proportional_hazard_test`.

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<TestResult> TestProportionalHazards(ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, CoxSummary fit, CoxTimeTransform transform = CoxTimeTransform.Rank)
```

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<TestResult> TestProportionalHazards(ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> weights, ReadOnlySpan<int> strata, CoxSummary fit, CoxTimeTransform transform)
```

The second overload takes the weights and strata a fit was made with.

**Parameters** — `design`, `durations` and `eventObserved` are what the model was fitted on, and
`weights` and `strata` too, or empty. `fit` is that model. `transform` is the
[`CoxTimeTransform`](coxtimetransform.md) the residuals are correlated with, the rank by default.

**Returns** — one [`TestResult`](../../stats/tests/testresult.md) per covariate, in the design's column
order: a chi-squared statistic on one degree of freedom and its p-value.

**Exceptions** — `ArgumentNullException` when `fit` is `null`. `ArgumentException` when the spans
disagree with each other or with the fit's covariate count, a value is not finite, no event is
observed, a weight or stratum span is neither empty nor one value per subject, the strata are not
the ones the fit was stratified by, or the fit is a `CoxTimeVarying` one, which lifelines' test does
not take either.
`ArgumentOutOfRangeException` when `transform` names no time scale.

**Example** — neither covariate drifts on this sample.

```csharp
using Lodestar.Stats;
using Lodestar.Survival;

double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary fit = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);
IReadOnlyList<TestResult> tests = CoxProportionalHazards.TestProportionalHazards(design, months, died, fit);

double first = Math.Round(tests[0].PValue, 6);    // => 0.326668
double second = Math.Round(tests[1].PValue, 6);   // => 0.945687
```

**Remarks** — **lifelines' approximation, which R left in 2019.** The Schoenfeld residual of each event
is its covariates less the Efron-weighted mean of its risk set at the fitted coefficients, scaled by
the number of events times the model-based covariance; covariate `k`'s statistic is
`(Σ gᵢ sᵢₖ)² / (d · seₖ² · Σ gᵢ²)`, `g` the transformed event times less their mean. The standard error
is the fit's own, robust where the fit was. So the numbers match lifelines and not a current
`cox.zph`. The rank transform counts events along the fit's sorted order, strata first, as lifelines
does; the Kaplan-Meier one pools the strata and takes the weights.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CoxTimeTransform`](coxtimetransform.md), [`CoxProportionalHazards.Fit`](coxproportionalhazards-fit.md).
