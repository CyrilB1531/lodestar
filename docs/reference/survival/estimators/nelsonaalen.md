# NelsonAalen

The Nelson-Aalen estimator of a cumulative hazard function.

<!-- docs-declaration -->

```csharp
public static class NelsonAalen
```

**Example** — the same trial arm, read as accumulated risk rather than as survival.

```csharp
using Lodestar.Survival;

double[] durations = [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35];
bool[] observed = [true, true, true, true, true, true, true, true, true,
                   false, false, false, false, false, false, false, false, false, false, false, false];

NelsonAalenCurve curve = NelsonAalen.Estimate(durations, observed);

double bySix = curve.CumulativeHazard[1];  // => 0.150250…
double bySeven = curve.CumulativeHazard[2];  // => 0.209074…
```

**Remarks** — a sum of hazard increments rather than a product of survival fractions. The two
estimators share a timeline because they share a risk table, and they are two readings of it.

**The difference shows at the end.** In a sample whose last duration is observed, Kaplan-Meier
reaches zero and can fall no further, while the hazard keeps the size of that last step. A curve
that has hit zero has lost the ability to distinguish "the study ended" from "everyone died"; the
hazard has not.

Reference behaviour is `lifelines.NelsonAalenFitter` 0.30.3 with its smoothing left off — the plain
estimator, which is what the fitter reports by default. Matched over 8 samples.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the estimators index](../estimators.md), [`KaplanMeier`](kaplanmeier.md).

## Members

| Member | What it does |
| --- | --- |
| [`NelsonAalen.Estimate`](nelsonaalen-estimate.md) | The cumulative hazard of a right-censored sample. |
