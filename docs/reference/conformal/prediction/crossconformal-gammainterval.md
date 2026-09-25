# CrossConformal.GammaInterval

The prediction interval at one test point for the gamma score, `conformity_score="gamma"` of
MAPIE's cross-conformal regressors.

<!-- docs-declaration -->

```csharp
public static (double Lower, double Upper) GammaInterval(ReadOnlySpan<double> testPredictions, ReadOnlySpan<bool> heldOut, ReadOnlySpan<double> scores, double alpha, CrossConformalMethod method = CrossConformalMethod.Plus, CrossConformalAggregation aggregation = CrossConformalAggregation.Mean)
```

<!-- docs-declaration -->

```csharp
public static (double Lower, double Upper) GammaInterval(ReadOnlySpan<double> testPredictions, ReadOnlySpan<int> folds, ReadOnlySpan<double> scores, double alpha, CrossConformalMethod method = CrossConformalMethod.Plus)
```

**Parameters** — `testPredictions` is each model's prediction at the test point, and every
held-out one must be strictly positive. `heldOut` is the `n × M` mask, or `folds` one model index
per training sample. `scores` come from [`SplitConformal.GammaScores`](splitconformal-gammascores.md).
`alpha` is the miscoverage level; `method` and `aggregation` are as for
[`CrossConformal.Interval`](crossconformal-interval.md).

**Returns** — the interval's two bounds, either of them infinite when there are too few samples for
its side's level.

**Exceptions** — `ArgumentOutOfRangeException` when `alpha` is not strictly between 0 and 1, when a fold is outside `[0, M)`, when an enum value is not declared,
or when a held-out prediction at the test point is not strictly positive; `ArgumentException` as
for `Interval`.

**Example** — relative errors, so the interval widens with the prediction.

```csharp
using Lodestar.Conformal;

// Six training samples in three folds; model m was fitted without fold m.
int[] folds = [0, 0, 1, 1, 2, 2];
double[] yTrue = [10.2, 11.5, 9.8, 12.6, 11.1, 10.4];
double[] outOfFold = [10.7, 10.5, 10.0, 11.8, 11.4, 11.0];   // each sample's prediction by its fold's model
double[] atTest = [10.0, 11.0, 12.0];                        // the three models' predictions at a new point

double[] scores = SplitConformal.GammaScores(yTrue, outOfFold);
(double lower, double upper) = CrossConformal.GammaInterval(atTest, folds, scores, 0.3);

double low = lower;     // => 9.532710280373832
double high = upper;    // => 11.745762711864405
```

**Remarks** — plus reads `ŷ₋ᵢ(x)(1 + Rᵢ)` over the training samples, min-max widens the smallest and
largest held-out prediction by `1 + q`. **The score is not symmetric**, so each side reads its own
quantile at `α/2`, as MAPIE's does; the lower side's level is evaluated as MAPIE evaluates it,
`(1 − 2a) + a`, which moves the rank by one at some sizes.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SplitConformal.GammaInterval`](splitconformal-gammainterval.md),
[`CrossConformal.Interval`](crossconformal-interval.md).
