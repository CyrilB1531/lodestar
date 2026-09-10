# KaplanMeier

The Kaplan-Meier estimator of a survival function.

<!-- docs-declaration -->

```csharp
public static class KaplanMeier
```

**Example** — the treatment arm of Freireich's leukaemia trial, the data every survival text opens
with.

```csharp
using Lodestar.Survival;

double[] durations = [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35];
bool[] observed = [true, true, true, true, true, true, true, true, true,
                   false, false, false, false, false, false, false, false, false, false, false, false];

KaplanMeierCurve curve = KaplanMeier.Estimate(durations, observed);

double atSix = curve.Survival[1];  // => 0.857142…
int atRisk = curve.Steps[1].AtRisk;  // => 21
```

**Remarks** — the estimate is the running product of `1 - d/n` over the times carrying an event.
Nine of those 21 subjects had the event; the other twelve were censored, and they contribute by
being in the risk set rather than by being dropped.

**A censoring leaves the risk set without moving the curve.** That step is still reported — it has
a `Censored` count and no `Events` — because a reader comparing against lifelines' event table
expects to see it, and because the next step's `AtRisk` only makes sense if it is there.

**Bounds are built on the log-log transform, not on the estimate.** This is lifelines' default and
it is not interchangeable with the plain Greenwood interval: at `S = 0.857` on 21 subjects the
latter reaches **1.0067**, outside the range a probability can take, where the transform gives
`[0.6197, 0.9516]`. The transform also means the bounds are not symmetric about the estimate.

Where the curve reaches zero the transform is undefined and both bounds collapse onto it — zero,
not `NaN`, which is what lifelines reports too.

Reference behaviour is `lifelines.KaplanMeierFitter` 0.30.3, matched over 8 samples.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the estimators index](../estimators.md), [`NelsonAalen`](nelsonaalen.md).

## Members

| Member | What it does |
| --- | --- |
| [`KaplanMeier.Estimate`](kaplanmeier-estimate.md) | The survival function of a right-censored sample. |
