# ParametricFit.SurvivalBounds

The delta-method interval of the survival function: lifelines'
`confidence_interval_survival_function_`.

<!-- docs-declaration -->

```csharp
public (double[] Lower, double[] Upper) SurvivalBounds(ReadOnlySpan<double> times)
```

**Parameters** — `times` are the positive times to read at.

**Returns** — the lower and upper bounds at `ConfidenceLevel`, one of each per time, not clipped to
`[0, 1]`.

**Example** — inside the data the interval is a probability; far past it, it is not.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

ParametricFit fit = ParametricSurvival.Fit(ParametricModel.Weibull, months, died);

(double[] lower, double[] upper) = fit.SurvivalBounds([10.0, 40.0]);
double atTen = Math.Round(lower[0], 6);         // => 0.297678
double pastTheData = Math.Round(lower[1], 6);   // => -0.015345
```

**Remarks** — **the interval is symmetric about the estimate on the survival scale**, `S ± z·σ`, with
`σ²` the gradient of `S` in the parameters through their covariance. That is lifelines' reading, and
it is why a bound can leave `[0, 1]`, as the one at forty months does; lifelines does not clip it,
and neither does this. [`CumulativeHazardBounds`](parametricfit-cumulativehazardbounds.md) is the
same interval on the hazard.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricFit`](parametricfit.md), [`ParametricFit.Survival`](parametricfit-survival.md).
