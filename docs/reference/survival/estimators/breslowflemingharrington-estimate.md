# BreslowFlemingHarrington.Estimate

The survival function of a right-censored sample, whose subjects may enter late.

<!-- docs-declaration -->

```csharp
public static SurvivalCurve Estimate(ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, double confidenceLevel = 0.95)
```

<!-- docs-declaration -->

```csharp
public static SurvivalCurve Estimate(ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> entries, double confidenceLevel = 0.95)
```

The second overload takes lifelines' `entry`.

**Parameters** — `durations` holds one non-negative time per subject, and `eventObserved` is `true`
where it ends in the event, `false` where the subject was censored at it. `entries` holds each
subject's entry time, non-negative and at most its duration, or is empty for everyone entering at
zero. `confidenceLevel` lies strictly inside `(0, 1)`.

**Returns** — a [`SurvivalCurve`](survivalcurve.md) at time zero, every duration and every entry
time, with its bounds.

**Exceptions** — `ArgumentException` when the spans differ in length, the sample is empty, a duration
is negative or `NaN`, or the entries are neither empty nor one valid time per subject.
`ArgumentOutOfRangeException` when `confidenceLevel` is not strictly inside `(0, 1)`.

**Example** — the first event, one in ten: `exp(−1/10)`, and its interval.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

SurvivalCurve curve = BreslowFlemingHarrington.Estimate(months, died);

double atThree = Math.Round(curve.Survival[1], 6);   // => 0.904837
double lower = Math.Round(curve.Lower[1], 6);        // => 0.49169
double upper = Math.Round(curve.Upper[1], 6);        // => 0.986012
```

**Remarks** — **a step with `d` events among `n` adds `1/n + … + 1/(n − d + 1)` to the hazard**, and
the squares of those to its variance, as lifelines' Nelson-Aalen fitter does with ties. The bounds
are `exp(−H e^{±z√V/H})`, the log transform of the hazard's interval carried through `exp(−·)`.

**`Lower` is the smaller bound.** lifelines labels the larger one its lower bound, being the image of
the hazard's lower bound; the two columns hold the same numbers, swapped.

**Late entrants join the risk set after the events at their entry time**, as lifelines counts them,
except at the first step; `SurvivalStep.AtRisk` reports them with the entrants, lifelines' `at_risk`.
Each entry time is a step of its own even where nothing happens at it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`BreslowFlemingHarrington`](breslowflemingharrington.md),
[`NelsonAalen.Estimate`](nelsonaalen-estimate.md), [`KaplanMeier.Estimate`](kaplanmeier-estimate.md).
