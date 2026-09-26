# ParametricSurvival.CompareAt

Tests whether two fitted survival functions differ at one time: lifelines'
`survival_difference_at_fixed_point_in_time_test` on two parametric fitters.

<!-- docs-declaration -->

```csharp
public static TestResult CompareAt(double time, ParametricFit fitA, ParametricFit fitB)
```

**Parameters** — `time` is the time both fits are read at, positive and finite. `fitA` and `fitB` are
two fits from [`ParametricSurvival`](parametricsurvival.md), of the same model or not.

**Returns** — a [`TestResult`](../../stats/tests/testresult.md): the chi-squared statistic on one
degree of freedom and its upper-tail p-value.

**Exceptions** — `ArgumentNullException` when a fit is `null`. `ArgumentOutOfRangeException` when
`time` is not positive and finite.

**Example** — two arms of ten, each fitted with a Weibull curve, compared at five months.

```csharp
using Lodestar.Stats;
using Lodestar.Survival;

double[] armA = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] diedA = [true, true, false, true, true, true, false, true, true, false];
double[] armB = [2, 4, 7, 1, 6, 3, 9, 5, 4, 8];
bool[] diedB = [true, true, true, true, false, true, true, true, true, false];

TestResult atFive = ParametricSurvival.CompareAt(5.0,
    ParametricSurvival.Fit(ParametricModel.Weibull, armA, diedA),
    ParametricSurvival.Fit(ParametricModel.Weibull, armB, diedB));

double statistic = Math.Round(atFive.Statistic, 6);   // => 5.84045
double p = Math.Round(atFive.PValue, 6);              // => 0.015662
```

**Remarks** — the statistic is [`KaplanMeier.CompareAt`](kaplanmeier-compareat.md)'s, on the
`log(−log S)` scale: `(log(−log S_A) − log(−log S_B))²` over `σ²_A/(log S_A)² + σ²_B/(log S_B)²`.

**`σ²` is the delta-method variance of `S` itself, not of `log S`.** A Kaplan-Meier curve hands the
formula Greenwood's variance of `log S`, which is what its denominator expects; lifelines hands it a
parametric fit's variance of `S` unchanged, and this keeps that reading so the statistic is the one
lifelines reports. The two differ by a factor of `S²` in each variance.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricFit.SurvivalBounds`](parametricfit-survivalbounds.md),
[`KaplanMeier.CompareAt`](kaplanmeier-compareat.md), [`LogRank`](logrank.md).
