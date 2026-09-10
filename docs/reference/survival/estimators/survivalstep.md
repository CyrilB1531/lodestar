# SurvivalStep

One step of a survival or cumulative-hazard curve.

<!-- docs-declaration -->

```csharp
public sealed record SurvivalStep(double Time, int AtRisk, int Events, int Censored)
```

**Parameters** — `Time` is the duration the step sits at. `AtRisk` is how many subjects were still
at risk immediately *before* it. `Events` is how many had the event at it, `Censored` how many left
without it.

**Example** — reading the risk set down a curve.

```csharp
using Lodestar.Survival;

KaplanMeierCurve curve = KaplanMeier.Estimate([1, 2, 3], [true, false, true]);

double opensAt = curve.Steps[0].Time;  // => 0
int everyone = curve.Steps[0].AtRisk;  // => 3
int afterCensoring = curve.Steps[2].AtRisk;  // => 2
```

**Remarks** — a step exists for **every distinct duration**, censorings included, so a time that
removes subjects without moving the estimate is still visible. `AtRisk` is measured before the step
rather than after, which is what makes `Events / AtRisk` the increment the estimators use.

The first step is always time zero with everyone at risk and nothing having happened — the shape
lifelines' own event table has, and what lets a curve be plotted without inventing a first point.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KaplanMeierCurve`](kaplanmeiercurve.md), [`NelsonAalenCurve`](nelsonaalencurve.md).
