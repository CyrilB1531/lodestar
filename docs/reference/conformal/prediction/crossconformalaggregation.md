# CrossConformalAggregation

How the predictions of several models that held a sample out combine into one: the
jackknife-after-bootstrap's `aggregation_method`.

<!-- docs-declaration -->

```csharp
public enum CrossConformalAggregation { Mean, Median }
```

**Members** — `Mean`, the default, and `Median`, the mean of the two middle predictions when their
count is even, as numpy's.

**Example** — the same bootstrap under both, at a test point.

```csharp
using Lodestar.Conformal;

// Four training samples, three bootstrap models; true where the model was fitted without the sample.
double[] trainPredictions = [10.1, 10.4, 9.9, 11.2, 11.0, 11.6, 9.5, 9.8, 9.7, 12.0, 12.4, 12.1];
bool[] heldOut = [true, true, true, false, true, true, true, true, false, false, false, true];

double[] yTrue = [10.3, 11.9, 9.2, 12.8];
double[] scores = SplitConformal.AbsoluteResiduals(yTrue, CrossConformal.OutOfSample(trainPredictions, heldOut, 3));
double[] atTest = [10.5, 10.9, 11.2];

(double low, double high) = CrossConformal.Interval(atTest, heldOut, scores, 0.4);

double lower = low;     // => 10.450000000000001
double upper = high;    // => 11.65
```

**Remarks** — under K-fold and leave-one-out a sample is held out by one model, and both members
give that model's prediction unchanged.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CrossConformal.OutOfSample`](crossconformal-outofsample.md).
