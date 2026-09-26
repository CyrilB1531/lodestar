# BreslowFlemingHarrington

The Breslow-Fleming-Harrington estimator of a survival function: the exponential of minus the
Nelson-Aalen hazard.

<!-- docs-declaration -->

```csharp
public static class BreslowFlemingHarrington
```

**Example** — where Kaplan-Meier reaches zero, this does not.

```csharp
using Lodestar.Survival;

SurvivalCurve smooth = BreslowFlemingHarrington.Estimate([1, 2, 3], [true, true, true]);
KaplanMeierCurve product = KaplanMeier.Estimate([1, 2, 3], [true, true, true]);

double bfhLast = Math.Round(smooth.Survival[3], 6);   // => 0.15988
double kmLast = product.Survival[3];                  // => 0
```

**Remarks** — `exp(−H)`, with `H` the [`NelsonAalen`](nelsonaalen.md) cumulative hazard, lies above
the Kaplan-Meier product at every step, by little while the risk set is large and by much at its end:
above, `exp(−1/3 − 1/2 − 1)` where the product's last factor is `1 − 1/1`. Prefer it on small samples
where a curve pinned to zero by the last subject is not believable; prefer
[`KaplanMeier`](kaplanmeier.md) everywhere else, being the estimator every reader expects.

Reference behaviour is `lifelines.BreslowFlemingHarringtonFitter` 0.30.3, right-censored with delayed
entry. **There are no weights**: lifelines' fitter passes none on to the Nelson-Aalen fit it wraps,
so its `weights` has no effect there. Thread-safe.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the estimators index](../estimators.md), [`SurvivalCurve`](survivalcurve.md),
[`NelsonAalen`](nelsonaalen.md), [Python → C# equivalence](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`BreslowFlemingHarrington.Estimate`](breslowflemingharrington-estimate.md) | The survival function of a right-censored sample, entries allowed. |
