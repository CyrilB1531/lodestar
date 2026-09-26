# ParametricFit.CumulativeHazardBounds

The delta-method interval of the cumulative hazard: lifelines'
`confidence_interval_cumulative_hazard_`.

<!-- docs-declaration -->

```csharp
public (double[] Lower, double[] Upper) CumulativeHazardBounds(ReadOnlySpan<double> times)
```

**Parameters** — `times` are the positive times to read at.

**Returns** — the lower and upper bounds at `ConfidenceLevel`, one of each per time.

**Example** — the interval at ten months.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

ParametricFit fit = ParametricSurvival.Fit(ParametricModel.Weibull, months, died);

(double[] lower, double[] upper) = fit.CumulativeHazardBounds([10.0]);
double low = Math.Round(lower[0], 6);    // => 0.121487
double high = Math.Round(upper[0], 6);   // => 1.051325
```

**Remarks** — `H ± z·σ`, with `σ²` the gradient of `H` in the parameters through their
covariance, as lifelines builds it. It is not the image of
[`SurvivalBounds`](parametricfit-survivalbounds.md) under `−log`: each is symmetric on its own scale.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricFit`](parametricfit.md), [`ParametricFit.CumulativeHazard`](parametricfit-cumulativehazard.md).
