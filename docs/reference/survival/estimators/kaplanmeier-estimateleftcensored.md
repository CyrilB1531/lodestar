# KaplanMeier.EstimateLeftCensored

The survival function of a left-censored sample: lifelines' `fit_left_censoring`.

<!-- docs-declaration -->

```csharp
public static KaplanMeierCurve EstimateLeftCensored(ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, double confidenceLevel = 0.95)
```

**Parameters** — `durations` holds one non-negative time per subject. `eventObserved` is `true` where
that time is the event's own, and `false` where the event happened at some unknown time before it;
the two spans must be the same length. `confidenceLevel` lies strictly inside `(0, 1)`.

**Returns** — a [`KaplanMeierCurve`](kaplanmeiercurve.md) on the steps
[`Estimate`](kaplanmeier-estimate.md) would build: one minus the reverse Kaplan-Meier estimate of the
cumulative density, with its bounds.

**Exceptions** — `ArgumentException` when the spans differ in length, the sample is empty, or a
duration is negative or `NaN`. `ArgumentOutOfRangeException` when `confidenceLevel` is not strictly
inside `(0, 1)`.

**Example** — ten readings, three of them only known to lie below a detection limit.

```csharp
using Lodestar.Survival;

double[] readings = [3, 5, 2, 8, 4, 6, 1, 7, 5, 9];
bool[] exact = [true, false, true, true, false, true, false, true, true, true];

KaplanMeierCurve curve = KaplanMeier.EstimateLeftCensored(readings, exact);

double atFive = Math.Round(curve.Survival[5], 6);   // => 0.4
double lower = Math.Round(curve.Lower[5], 6);       // => 0.172779
double upper = Math.Round(curve.Upper[5], 6);       // => 0.747331
```

**Remarks** — **the time axis is read backwards.** At each step the cumulative density is the product
of `1 − d/n` over the later times, `n` counting the subjects whose duration is at most that time, and
its log-log interval takes Greenwood's sum over the same times. The survival is one minus that
density. A subject enters `n` at its own time and stays for every later one, the mirror of a
right-censored subject leaving the risk set.

**The curve need not start at one.** When the smallest duration is itself left-censored, some mass
lies before every observed time and the density at zero is already positive: above, the reading
censored at 1 puts the curve at 0.833 at time zero.

**`Lower` is the smaller bound.** The survival's bounds are one minus the density's, and lifelines
names the larger of them its lower bound; the two columns hold the same numbers, swapped.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KaplanMeier`](kaplanmeier.md), [`KaplanMeier.Estimate`](kaplanmeier-estimate.md),
[`ParametricSurvival.FitLeftCensored`](parametricsurvival-fitleftcensored.md).
