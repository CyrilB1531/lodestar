# ParametricFit.Hazard

The fitted hazard at given times: lifelines' `hazard_at_times`.

<!-- docs-declaration -->

```csharp
public double[] Hazard(ReadOnlySpan<double> times)
```

**Parameters** — `times` are the positive times to read at.

**Returns** — one instantaneous hazard per time, the derivative of the cumulative hazard.

**Example** — a Weibull shape above one: the hazard rises with time.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

ParametricFit fit = ParametricSurvival.Fit(ParametricModel.Weibull, months, died);

double[] hazard = fit.Hazard([5.0, 10.0]);
double atFive = Math.Round(hazard[0], 6);   // => 0.06135
double atTen = Math.Round(hazard[1], 6);    // => 0.099833
```

**Remarks** — this is what a non-parametric estimate cannot give without smoothing:
[`NelsonAalen`](nelsonaalen.md) reports the accumulated hazard as steps, whose derivative is zero
between events and undefined at them.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricFit`](parametricfit.md), [`ParametricFit.CumulativeHazard`](parametricfit-cumulativehazard.md).
