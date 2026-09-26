# AalenAdditive

Aalen's additive hazards model: each covariate adds to the hazard, by an amount that moves with time.

<!-- docs-declaration -->

```csharp
public static class AalenAdditive
```

**Example** — how much a treatment takes off the hazard, month by month, on twelve patients.

```csharp
using Lodestar.Survival;

// 1 if on the new treatment; months to death or to the end of follow-up.
double[] treated = [0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1];
double[] months = [3, 10, 5, 4, 8, 18, 12, 6, 9, 20, 7, 15];
bool[] died = [true, true, true, false, true, true, true, true, true, false, true, true];

AalenSummary fit = AalenAdditive.Fit(treated, months, died, featureCount: 1);

double perMonth = fit.Slopes[0];              // => -0.200022…
double concordance = fit.ConcordanceIndex;    // => 0.598214…
```

**Remarks** — **the model assumes nothing about how an effect changes with time.** Where
[`CoxProportionalHazards`](coxproportionalhazards.md) fixes each covariate's hazard ratio for the
whole follow-up, this estimates a separate increment at every event time, and reports their running
sum: a cumulative coefficient whose slope at a time is the covariate's effect then. A straight line
means a constant effect; a bend means the effect moved. The slope above says the treatment takes
about 0.2 a month off the hazard, averaged over the follow-up.

The price is that nothing keeps the hazard positive. An increment may take either sign, so a
predicted cumulative hazard may fall and a predicted survival curve rise.

**At each distinct event time the increments are a ridge-penalised least squares** of that time's
deaths on the covariates of the subjects at risk, the covariates scaled to unit sample deviation and
weighted by the square roots of the weights. [`AalenOptions`](aalenoptions.md) sets the two penalties
and the intercept, which, when fitted, is the last coefficient.

Reference behaviour is `lifelines` 0.30.3's `AalenAdditiveFitter`, whose readings are kept on
purpose; [`AalenAdditive.Fit`](aalenadditive-fit.md) lists them. Thread-safe.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the estimators index](../estimators.md), [`AalenSummary`](aalensummary.md),
[`AalenOptions`](aalenoptions.md), [Python → C# equivalence](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`AalenAdditive.Fit`](aalenadditive-fit.md) | Fits the model to right-censored durations. |
