# AalenSummary.SmoothedHazards

The increments smoothed by an Epanechnikov kernel: lifelines' `smoothed_hazards_`.

<!-- docs-declaration -->

```csharp
public double[] SmoothedHazards(double bandwidth = 1)
```

**Parameters** — `bandwidth` is the kernel's half-width, in time; positive and finite, one by
default, lifelines' default.

**Returns** — row-major, one row per [`EventTimes`](aalensummary.md) entry and one column per
coefficient, as `Hazards`.

**Exceptions** — `ArgumentOutOfRangeException` when `bandwidth` is not positive and finite.

**Example** — the treatment's smoothed increment at the first event time, over three months either
side.

```csharp
using Lodestar.Survival;

double[] treated = [0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1];
double[] months = [3, 10, 5, 4, 8, 18, 12, 6, 9, 20, 7, 15];
bool[] died = [true, true, true, false, true, true, true, true, true, false, true, true];

AalenSummary fit = AalenAdditive.Fit(treated, months, died, featureCount: 1);

double[] smoothed = fit.SmoothedHazards(3.0);
double atThree = Math.Round(smoothed[0], 6);   // => -0.208333
```

**Remarks** — at each event time, the sum of every increment within `bandwidth` of it, weighted by
`0.75 (1 − u²)` with `u` the gap over the bandwidth. The kernel is not divided by the bandwidth,
as lifelines' is not, so the result is a sum of increments rather than a hazard per unit of time.
**A bandwidth no wider than the gap between event times smooths nothing**: each time then sees only
its own increment, times 0.75, which is what the default does on durations recorded in whole
months.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AalenSummary`](aalensummary.md), [`AalenAdditive`](aalenadditive.md).
