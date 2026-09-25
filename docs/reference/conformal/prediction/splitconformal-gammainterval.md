# SplitConformal.GammaInterval

The gamma interval `[ŷ(1 + q_low), ŷ(1 + q_up)]` around a strictly positive prediction,
`SplitConformalRegressor(..., conformity_score="gamma")`.

<!-- docs-declaration -->

```csharp
public static (double Lower, double Upper) GammaInterval(double prediction, ReadOnlySpan<double> scores, double alpha)
```

**Parameters** — `prediction` is the model's point prediction, strictly positive. `scores` are the
calibration scores from [`GammaScores`](splitconformal-gammascores.md). `alpha` is the miscoverage
level.

**Returns** — the interval's two bounds, either of them infinite when the calibration set is too
small for its side's level.

**Exceptions** — `ArgumentException` when `scores` is empty or holds a `NaN`;
`ArgumentOutOfRangeException` when `alpha` is not strictly between 0 and 1, or `prediction` is not
strictly positive.

**Example** — six calibration points at 70 % coverage.

```csharp
using Lodestar.Conformal;

double[] scores = SplitConformal.GammaScores(
    [10.2, 11.5, 9.8, 12.6, 11.1, 10.4], [10.7, 10.5, 10.0, 11.8, 11.4, 11.0]);

(double lower, double upper) = SplitConformal.GammaInterval(11.0, scores, 0.3);

double low = lower;     // => 10.4
double high = upper;    // => 12.04761904761905
```

**Remarks** — it takes the scores rather than a quantile, as [`Interval`](splitconformal-interval.md)
does, because **the score is not symmetric**: each side reads its own quantile at `α/2`, as MAPIE's
does, the lower one at MAPIE's own floating-point level. MAPIE's cross-conformal `method="base"` is
this interval over the out-of-sample scores.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SplitConformal.GammaScores`](splitconformal-gammascores.md),
[`CrossConformal.GammaInterval`](crossconformal-gammainterval.md).
