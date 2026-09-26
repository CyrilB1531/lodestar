# ParametricFit.Survival

The fitted survival function at given times: lifelines' `survival_function_at_times`.

<!-- docs-declaration -->

```csharp
public double[] Survival(ReadOnlySpan<double> times)
```

**Parameters** — `times` are the positive times to read at.

**Returns** — one survival probability per time, `exp(−H(t))`.

**Example** — the Weibull fit of the [`ParametricSurvival`](parametricsurvival.md) page at ten months.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

ParametricFit fit = ParametricSurvival.Fit(ParametricModel.Weibull, months, died);

double atTen = Math.Round(fit.Survival([10.0])[0], 6);   // => 0.556323
```

**Remarks** — the curve is defined at every time, past the last duration too, which is where it is
least to be trusted: the model's shape, not the data, says what happens there.
[`SurvivalBounds`](parametricfit-survivalbounds.md) gives its interval.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricFit`](parametricfit.md), [`ParametricFit.CumulativeHazard`](parametricfit-cumulativehazard.md).
