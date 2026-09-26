# ParametricFit.Percentile

The time by which the fitted survival falls to a level: lifelines' `percentile`.

<!-- docs-declaration -->

```csharp
public double Percentile(double probability)
```

**Parameters** — `probability` is the survival level, strictly inside `(0, 1)`.

**Returns** — the time at which the survival function equals `probability`.

**Exceptions** — `ArgumentOutOfRangeException` when `probability` is not strictly inside `(0, 1)`.

**Example** — the level is a survival, so a quarter lies later than the median.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

ParametricFit fit = ParametricSurvival.Fit(ParametricModel.Weibull, months, died);

double quarterLeft = Math.Round(fit.Percentile(0.25), 6);    // => 16.57602
double median = Math.Round(fit.MedianSurvivalTime, 6);       // => 11.032146
```

**Remarks** — **the argument is the probability of surviving, not of having had the event**, as in
lifelines: `Percentile(0.25)` is the time by which three quarters have had it. `MedianSurvivalTime`
is `Percentile(0.5)`. Every model inverts in closed form except the generalized gamma, which inverts
its incomplete gamma function.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricFit`](parametricfit.md), [`AftSummary.PredictPercentile`](aftsummary-predictpercentile.md).
