# SurvivalCurve

A survival curve with its confidence bounds, one value per step: what the Breslow-Fleming-Harrington
estimator returns.

<!-- docs-declaration -->

```csharp
public sealed record SurvivalCurve(SurvivalStep[] Steps, double[] Survival, double[] Lower, double[] Upper, double ConfidenceLevel)
```

**Parameters** — `Steps` are the curve's steps, ascending in time and starting at zero. `Survival`,
`Lower` and `Upper` hold the estimate and its two bounds, one entry per step, `Lower` the smaller.
`ConfidenceLevel` is the level the bounds were built at.

**Example** — the four arrays share one index.

```csharp
using Lodestar.Survival;

SurvivalCurve curve = BreslowFlemingHarrington.Estimate([1, 2, 3], [true, true, true]);

int steps = curve.Steps.Length;                          // => 4
double level = curve.ConfidenceLevel;                    // => 0.95
double lastSurvival = Math.Round(curve.Survival[3], 6);  // => 0.15988
```

**Remarks** — the shape of [`KaplanMeierCurve`](kaplanmeiercurve.md), the bounds excepted: these are
the Nelson-Aalen hazard's interval on the log scale, carried through `exp(−·)`, where Kaplan-Meier's
are Greenwood's. A separate type keeps one curve from being passed where the other's variance is
assumed, as [`KaplanMeier.CompareAt`](kaplanmeier-compareat.md) assumes Greenwood's.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`BreslowFlemingHarrington.Estimate`](breslowflemingharrington-estimate.md),
[`SurvivalStep`](survivalstep.md).

## Members

| member | what it does |
| --- | --- |
| [`SurvivalCurve.Equals`](survivalcurve-equals.md) | Value equality over the steps and the three curves. |
| [`SurvivalCurve.GetHashCode`](survivalcurve-gethashcode.md) | A hash consistent with it. |
