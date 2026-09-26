# KaplanMeier.CompareAt

Tests whether two survival curves differ at one time: lifelines'
`survival_difference_at_fixed_point_in_time_test`.

<!-- docs-declaration -->

```csharp
public static TestResult CompareAt(double time, KaplanMeierCurve curveA, KaplanMeierCurve curveB)
```

**Parameters** — `time` is the time both curves are read at, non-negative. `curveA` and `curveB` are
two curves from [`KaplanMeier.Estimate`](kaplanmeier-estimate.md).

**Returns** — a [`TestResult`](../../stats/tests/testresult.md): the chi-squared statistic on one
degree of freedom and its upper-tail p-value.

**Exceptions** — `ArgumentNullException` when a curve is `null`, and `ArgumentException` when one
holds no step or not one estimate per step. `ArgumentOutOfRangeException` when
`time` is negative or `NaN`.

**Example** — Freireich's arms at week 10, where the log-rank test reads the whole curve instead.

```csharp
using Lodestar.Stats;
using Lodestar.Survival;

double[] treatment = [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35];
bool[] treatmentObserved = [true, true, true, true, true, true, true, true, true,
                            false, false, false, false, false, false, false, false, false, false, false, false];
double[] control = [1, 1, 2, 2, 3, 4, 4, 5, 5, 8, 8, 8, 8, 11, 11, 12, 12, 15, 17, 22, 23];
bool[] controlObserved = [true, true, true, true, true, true, true, true, true, true, true,
                          true, true, true, true, true, true, true, true, true, true];

TestResult atTen = KaplanMeier.CompareAt(10,
    KaplanMeier.Estimate(treatment, treatmentObserved), KaplanMeier.Estimate(control, controlObserved));

double statistic = Math.Round(atTen.Statistic, 6);   // => 4.737596
double p = Math.Round(atTen.PValue, 6);              // => 0.02951
```

**Remarks** — Klein, Logan, Harhoff and Andersen's test on the `log(−log S)` scale: the squared
difference of the two transformed estimates over the sum of their delta-method variances.

**Two readings of the curve, as lifelines takes them.** Each estimate is the curve's step at `time`,
while its Greenwood sum is **interpolated linearly** between the times on either side, which is
`numpy.interp` over lifelines' `_cumulative_sq_`. A step where every subject left at risk has the
event adds nothing to that sum, where [`KaplanMeier.Estimate`](kaplanmeier-estimate.md)'s bounds
read it as infinite. A curve at one or at zero at `time` leaves the transform undefined, and the
statistic is `NaN`, as lifelines' is.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KaplanMeier.RestrictedMean`](kaplanmeier-restrictedmean.md), [`LogRank`](logrank.md).
