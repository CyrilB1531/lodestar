# KaplanMeier.RestrictedMean

The restricted mean survival time of a curve, with its variance: lifelines'
`restricted_mean_survival_time`.

<!-- docs-declaration -->

```csharp
public static RestrictedMeanResult RestrictedMean(KaplanMeierCurve curve)
```

<!-- docs-declaration -->

```csharp
public static RestrictedMeanResult RestrictedMean(KaplanMeierCurve curve, double horizon)
```

The first overload integrates without a limit, lifelines' default `t=inf`.

**Parameters** — `curve` is a curve from [`KaplanMeier.Estimate`](kaplanmeier-estimate.md).
`horizon` is the upper limit of the integral, lifelines' `t`: non-negative, and infinity allowed.

**Returns** — a [`RestrictedMeanResult`](restrictedmeanresult.md): the area under the curve up to
`horizon`, and its variance.

**Exceptions** — `ArgumentNullException` when `curve` is `null`, and `ArgumentException` when it
holds no step or not one estimate per step. `ArgumentOutOfRangeException` when
`horizon` is negative or `NaN`.

**Example** — Freireich's control arm, up to week 10 and without a limit.

```csharp
using Lodestar.Survival;

double[] control = [1, 1, 2, 2, 3, 4, 4, 5, 5, 8, 8, 8, 8, 11, 11, 12, 12, 15, 17, 22, 23];
bool[] controlObserved = [true, true, true, true, true, true, true, true, true, true, true,
                          true, true, true, true, true, true, true, true, true, true];
KaplanMeierCurve curve = KaplanMeier.Estimate(control, controlObserved);

RestrictedMeanResult tenWeeks = KaplanMeier.RestrictedMean(curve, 10);
RestrictedMeanResult whole = KaplanMeier.RestrictedMean(curve);

double mean = Math.Round(tenWeeks.Mean, 6);           // => 6.619048
double variance = Math.Round(tenWeeks.Variance, 6);   // => 11.283447
double unrestricted = Math.Round(whole.Mean, 6);      // => 8.666667
double spread = Math.Round(whole.Variance, 6);        // => 39.84127
```

**Remarks** — the mean is `∫ S` up to the horizon and the variance `2 ∫ τ S(τ) dτ` less the squared
mean, as lifelines defines them, both **summed exactly over the steps**.

**A measured divergence in lifelines' variance.** lifelines sums the mean the same way, but integrates
the second moment by `scipy.integrate.quad` over the step function, which lands up to **1.5 %**
relative from the exact integral over 300 random curves: on the whole control arm above it reports
`39.786` where the area is `39.841`. This method returns the exact integral, the corpus freezes both,
and the mean matches lifelines to the last digit.

A curve still above zero at its last step never closes its area, so an infinite horizon gives an
infinite mean and variance unless the curve reaches zero, as this one does.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RestrictedMeanResult`](restrictedmeanresult.md), [`KaplanMeier.CompareAt`](kaplanmeier-compareat.md).
