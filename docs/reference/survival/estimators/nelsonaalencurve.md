# NelsonAalenCurve

A Nelson-Aalen cumulative-hazard curve.

<!-- docs-declaration -->

```csharp
public sealed record NelsonAalenCurve(SurvivalStep[] Steps, double[] CumulativeHazard)
```

**Parameters** — `Steps` are the curve's steps, ascending in time and starting at zero.
`CumulativeHazard` holds the accumulated hazard, one entry per step.

**Example** — the hazard keeps rising where survival cannot fall further.

```csharp
using Lodestar.Survival;

NelsonAalenCurve curve = NelsonAalen.Estimate([1, 2, 3], [true, true, true]);

double atStart = curve.CumulativeHazard[0];  // => 0
double atEnd = curve.CumulativeHazard[3];  // => 1.833…
```

**Remarks** — `Steps` is the same table [`KaplanMeierCurve`](kaplanmeiercurve.md) carries, because
both estimators are built on one risk table. Comparing the two on the same sample is therefore an
index-by-index comparison with no alignment step.

The last value above is `1/3 + 1/2 + 1/1`. Kaplan-Meier on that sample ends at zero; this ends at
1.833, and the difference is the information a curve pinned to zero has lost.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NelsonAalen.Estimate`](nelsonaalen-estimate.md), [`SurvivalStep`](survivalstep.md).

## Members

| member | what it does |
| --- | --- |
| [`NelsonAalenCurve.Equals`](nelsonaalencurve-equals.md) | Value equality over the steps and the hazard. |
| [`NelsonAalenCurve.GetHashCode`](nelsonaalencurve-gethashcode.md) | A hash consistent with it. |
