# RestrictedMeanResult

The restricted mean survival time of a curve, with its variance: what
[`KaplanMeier.RestrictedMean`](kaplanmeier-restrictedmean.md) returns.

<!-- docs-declaration -->

```csharp
public sealed record RestrictedMeanResult(double Mean, double Variance)
```

**Properties** — `Mean` is the area under the survival curve up to the horizon, lifelines' RMST.
`Variance` is the restricted second moment less the squared mean, as lifelines defines it.

**Example** — one subject, dead at time two: two time units of survival, all of them certain.

```csharp
using Lodestar.Survival;

RestrictedMeanResult result = KaplanMeier.RestrictedMean(KaplanMeier.Estimate([2], [true]));

double mean = result.Mean;          // => 2
double variance = result.Variance;  // => 0
```

**Remarks** — the variance is that of the survival time restricted to the horizon under the fitted
curve, not the sampling variance of the mean; lifelines names it the same way.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KaplanMeier.RestrictedMean`](kaplanmeier-restrictedmean.md).
