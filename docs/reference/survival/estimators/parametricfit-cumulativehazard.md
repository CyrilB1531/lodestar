# ParametricFit.CumulativeHazard

The fitted cumulative hazard at given times: lifelines' `cumulative_hazard_at_times`.

<!-- docs-declaration -->

```csharp
public double[] CumulativeHazard(ReadOnlySpan<double> times)
```

**Parameters** — `times` are the positive times to read at.

**Returns** — one cumulative hazard per time.

**Example** — for the Weibull, `(t / λ)^ρ`.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

ParametricFit fit = ParametricSurvival.Fit(ParametricModel.Weibull, months, died);

double atTen = Math.Round(fit.CumulativeHazard([10.0])[0], 6);   // => 0.586406
```

**Remarks** — [`Survival`](parametricfit-survival.md) is its `exp(−H)`, so the two carry the same
information; the hazard is the scale on which models are written, and on which
[`CumulativeHazardBounds`](parametricfit-cumulativehazardbounds.md) stays positive where the survival
interval would not.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricFit`](parametricfit.md), [`ParametricFit.Hazard`](parametricfit-hazard.md).
