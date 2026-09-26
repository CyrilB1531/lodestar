# CoxProportionalHazards

The Cox proportional hazards model, fitted by Newton-Raphson on Efron's partial likelihood.

<!-- docs-declaration -->

```csharp
public static class CoxProportionalHazards
```

**Example** — how much a dose and a treatment move the hazard, on ten patients.

```csharp
using Lodestar.Survival;

// One row per patient: dose in mg, then 1 if on the new treatment.
double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary summary = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);

double perMilligram = summary.HazardRatios[0];  // => 4.0687…
double p = summary.PValues[0];                  // => 0.02669…
```

**Remarks** — **the model assumes the covariates multiply the hazard by the same factor at every
time.** That is the "proportional" in its name, and a hazard ratio of 4.07 per milligram means
nothing if the dose's effect fades after the first months.
[`TestProportionalHazards`](coxproportionalhazards-testproportionalhazards.md) tests it covariate
by covariate, and [the guide](../../../guides/survival-analysis.md) says how to read a failure.

It answers a different question from its neighbours. [`KaplanMeier`](kaplanmeier.md) says what
survival looks like, and [`LogRank`](logrank.md) whether two groups differ. This says by how much
each covariate changes the hazard, holding the others fixed.

Reference behaviour is `lifelines.CoxPHFitter` 0.30.3, right-censored, with its strata, subject
weights, elastic-net penalty, robust and clustered variances, baselines and predictions. The divergences are recorded in
[decision 0003](../../../decisions/0003-the-package-layout-tiers-boundaries-and-edges.md)
and the [equivalence table](../../../equivalence.md).

**Applies to** — net10.0, netstandard2.0.

**See also** — [the estimators index](../estimators.md), [`CoxSummary`](coxsummary.md),
[`CoxOptions`](coxoptions.md).

## Members

| Member | What it does |
| --- | --- |
| [`CoxProportionalHazards.Fit`](coxproportionalhazards-fit.md) | Fits the model and reports its inference table. |
| [`CoxProportionalHazards.TestProportionalHazards`](coxproportionalhazards-testproportionalhazards.md) | Tests each covariate for a hazard ratio that drifts with time. |
