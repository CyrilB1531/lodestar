# CrossConformal.Interval

The prediction interval at one test point for the absolute-residual score, MAPIE's
`CrossConformalRegressor(...).predict_interval` and `JackknifeAfterBootstrapRegressor`'s.

<!-- docs-declaration -->

```csharp
public static (double Lower, double Upper) Interval(ReadOnlySpan<double> testPredictions, ReadOnlySpan<bool> heldOut, ReadOnlySpan<double> scores, double alpha, CrossConformalMethod method = CrossConformalMethod.Plus, CrossConformalAggregation aggregation = CrossConformalAggregation.Mean)
```

<!-- docs-declaration -->

```csharp
public static (double Lower, double Upper) Interval(ReadOnlySpan<double> testPredictions, ReadOnlySpan<int> folds, ReadOnlySpan<double> scores, double alpha, CrossConformalMethod method = CrossConformalMethod.Plus)
```

The second overload is CV+ and Jackknife+, where each training sample is held out by exactly one
model: `folds[i]` names it, and the interval is the mask overload's with one `true` per row.

**Parameters** — `testPredictions` is each model's prediction at the test point. `heldOut` is the
`n × M` mask [`OutOfSample`](crossconformal-outofsample.md) takes, or `folds` one model index per
training sample. `scores` are the training samples' absolute residuals from their out-of-sample
predictions. `alpha` is the miscoverage level; `method` and `aggregation` are
[`CrossConformalMethod`](crossconformalmethod.md) and
[`CrossConformalAggregation`](crossconformalaggregation.md).

**Returns** — the interval's two bounds, either of them infinite when there are too few samples for
the level.

**Exceptions** — `ArgumentOutOfRangeException` when `alpha` is not strictly between 0 and 1, when a fold is outside `[0, M)`, or when an enum value is not
declared; `ArgumentException` when the lengths disagree, when no sample is held out, when a
test prediction is not finite, or when a held-out sample's score is `NaN`.

**Example** — the same three folds under plus and under min-max.

```csharp
using Lodestar.Conformal;

// Six training samples in three folds; model m was fitted without fold m.
int[] folds = [0, 0, 1, 1, 2, 2];
double[] yTrue = [10.2, 11.5, 9.8, 12.6, 11.1, 10.4];
double[] outOfFold = [10.7, 10.5, 10.0, 11.8, 11.4, 11.0];   // each sample's prediction by its fold's model
double[] atTest = [10.0, 11.0, 12.0];                        // the three models' predictions at a new point

double[] scores = SplitConformal.AbsoluteResiduals(yTrue, outOfFold);

(double plusLow, double plusHigh) = CrossConformal.Interval(atTest, folds, scores, 0.3);
(double minMaxLow, double minMaxHigh) = CrossConformal.Interval(atTest, folds, scores, 0.3, CrossConformalMethod.MinMax);

double plusWidth = plusHigh - plusLow;          // => 2.8000000000000007
double minMaxWidth = minMaxHigh - minMaxLow;    // => 3.599999999999998
```

**Remarks** — **plus** reads the `α` quantile of `ŷ₋ᵢ(x) − Rᵢ` and the `1 − α` quantile of
`ŷ₋ᵢ(x) + Rᵢ` over the training samples, `ŷ₋ᵢ(x)` the prediction at the test point of the
models fitted without sample `i`; **min-max** widens the smallest and the largest of those by the
scores' quantile, and is never narrower. The quantile is MAPIE's ceiling rank read at its own
floating-point level, so the bounds agree with it to the last bit, and **a rank past the samples is
an infinite bound** where MAPIE raises — see [`decisions/0007`](../../../decisions/0007-the-deliberate-divergences.md).
A sample no model holds out is skipped by both methods, where MAPIE's min-max returns `NaN` for
every test point.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CrossConformal.GammaInterval`](crossconformal-gammainterval.md),
[`SplitConformal.Interval`](splitconformal-interval.md).
