# KaplanMeier.Estimate

The survival function of a right-censored sample.

<!-- docs-declaration -->

```csharp
public static KaplanMeierCurve Estimate(ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, double confidenceLevel = 0.95)
```

**Parameters** — `durations` holds one non-negative time per subject. `eventObserved` is `true`
where that time ends in the event and `false` where the subject was censored at it; the two spans
must be the same length. `confidenceLevel` lies strictly inside `(0, 1)`.

**Returns** — a `KaplanMeierCurve` whose `Steps`, `Survival`, `Lower` and `Upper` share one index.

**Exceptions** — `ArgumentException` when the spans differ in length, the sample is empty, or a
duration is negative or `NaN`. `ArgumentOutOfRangeException` when `confidenceLevel` is not strictly
inside `(0, 1)`.

**Example** — a small sample, read step by step.

```csharp
using Lodestar.Survival;

KaplanMeierCurve curve = KaplanMeier.Estimate([1, 2, 3], [true, false, true]);

double opening = curve.Survival[0];  // => 1
double afterFirst = curve.Survival[1];  // => 0.666…
int stillAtRisk = curve.Steps[2].AtRisk;  // => 2
```

**Remarks** — the second step is `1 - 1/3`. The third carries the censoring at time 2, so the
estimate does not move there while `AtRisk` falls from 3 to 2 — and the fourth then divides by that
smaller risk set, which is the whole mechanism by which a censored subject still counts.

**The confidence level is a level, not a multiplier.** `0.95` asks for the two-sided 95% interval,
so the critical value taken is the normal quantile at `0.975`. That quantile is
[`Distributions.NormalQuantile`](../../stats/tails/distributions-normalquantile.md) rather than a
Student one at a large degrees of freedom: the substitute's accuracy peaks around `9e-9` and the
log-log transform amplifies that into the seventh digit of a bound —
[decision 0098](../../../decisions/0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md)
has the measurement, and this corpus is what caught it.

**Greenwood's sum is accumulated, not the variance.** The variance of the estimate is `S²` times
that sum, but the log-log interval needs the sum alone, so it is what the loop carries. When the
last subjects all have the event the increment is not finite; the sum becomes infinite, the estimate
is zero, and both bounds collapse there.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KaplanMeier`](kaplanmeier.md), [`NelsonAalen.Estimate`](nelsonaalen-estimate.md).
