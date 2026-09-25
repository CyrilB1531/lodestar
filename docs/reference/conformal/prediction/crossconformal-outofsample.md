# CrossConformal.OutOfSample

Each training sample's out-of-sample prediction: the aggregate of the models fitted without it.

<!-- docs-declaration -->

```csharp
public static double[] OutOfSample(ReadOnlySpan<double> predictions, ReadOnlySpan<bool> heldOut, int modelCount, CrossConformalAggregation aggregation = CrossConformalAggregation.Mean)
```

**Parameters** — `predictions` is every model's prediction at every training sample, row-major,
`n` rows of `modelCount` values. `heldOut` has the same shape: `true` where model `m` was fitted
without sample `i`. `aggregation` combines the models that held a sample out, MAPIE's
`aggregation_method`.

**Returns** — one prediction per training sample, `NaN` for a sample no model held out. Score them
with [`SplitConformal.AbsoluteResiduals`](splitconformal-absoluteresiduals.md) or
[`SplitConformal.GammaScores`](splitconformal-gammascores.md).

**Exceptions** — `ArgumentOutOfRangeException` when `modelCount` is not positive or `aggregation` is
not declared; `ArgumentException` when the two blocks differ in length or are not a multiple of
`modelCount`.

**Example** — the jackknife-after-bootstrap's out-of-bag predictions, by mean and by median.

```csharp
using Lodestar.Conformal;

// Four training samples, three bootstrap models; true where the model was fitted without the sample.
double[] trainPredictions = [10.1, 10.4, 9.9, 11.2, 11.0, 11.6, 9.5, 9.8, 9.7, 12.0, 12.4, 12.1];
bool[] heldOut = [true, true, true, false, true, true, true, true, false, false, false, true];

double[] mean = CrossConformal.OutOfSample(trainPredictions, heldOut, 3);
double[] median = CrossConformal.OutOfSample(trainPredictions, heldOut, 3, CrossConformalAggregation.Median);

double first = mean[0];          // => 10.133333333333333
double middle = median[0];       // => 10.1
double single = mean[3];         // => 12.1
```

**Remarks** — under K-fold and leave-one-out each sample is held out by one model, and its
out-of-fold prediction is simply that model's; this call is for the bootstrap, where a sample is
out of bag in several. **A sample in every bag has no out-of-sample prediction**: its `NaN` is
skipped by the intervals through the same mask, as MAPIE's `plus` skips it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CrossConformal.Interval`](crossconformal-interval.md),
[`CrossConformalAggregation`](crossconformalaggregation.md).
