# KaplanMeierCurve

A Kaplan-Meier survival curve with its Greenwood variance and interval.

<!-- docs-declaration -->

```csharp
public sealed record KaplanMeierCurve(SurvivalStep[] Steps, double[] Survival, double[] Lower, double[] Upper, double ConfidenceLevel)
```

**Parameters** — `Steps` are the curve's steps, ascending in time and starting at zero. `Survival`,
`Lower` and `Upper` hold the estimate and its two bounds, one entry per step. `ConfidenceLevel` is
the level the bounds were built at.

**Example** — the four arrays share one index.

```csharp
using Lodestar.Survival;

KaplanMeierCurve curve = KaplanMeier.Estimate([1, 2, 3], [true, true, true]);

int steps = curve.Steps.Length;  // => 4
double level = curve.ConfidenceLevel;  // => 0.95
double lastSurvival = curve.Survival[3];  // => 0
```

**Remarks** — the bounds are built on the **log-log transform** of the estimate, which is lifelines'
default. They are therefore **not symmetric** about `Survival`, and they are not the estimate plus
or minus its own standard error — that interval can leave `[0, 1]`, and at `S = 0.857` on 21
subjects it reaches 1.0067.

Where the curve reaches zero, as above, the transform is undefined and both bounds collapse onto the
estimate.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KaplanMeier.Estimate`](kaplanmeier-estimate.md), [`SurvivalStep`](survivalstep.md).
