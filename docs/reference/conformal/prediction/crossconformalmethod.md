# CrossConformalMethod

How [`CrossConformal`](crossconformal.md) turns the out-of-sample predictions at a test point into
an interval: MAPIE's `method`.

<!-- docs-declaration -->

```csharp
public enum CrossConformalMethod { Plus, MinMax }
```

**Members** — `Plus`, the default, is CV+ and Jackknife+: quantiles over the training samples of
each one's held-out prediction widened by its own score. `MinMax` widens the smallest and the
largest held-out prediction by the scores' quantile.

**Example** — min-max is never the narrower of the two.

```csharp
using Lodestar.Conformal;

// Six training samples in three folds; model m was fitted without fold m.
int[] folds = [0, 0, 1, 1, 2, 2];
double[] yTrue = [10.2, 11.5, 9.8, 12.6, 11.1, 10.4];
double[] outOfFold = [10.7, 10.5, 10.0, 11.8, 11.4, 11.0];   // each sample's prediction by its fold's model
double[] atTest = [10.0, 11.0, 12.0];                        // the three models' predictions at a new point

double[] scores = SplitConformal.AbsoluteResiduals(yTrue, outOfFold);

(double low, double high) = CrossConformal.Interval(atTest, folds, scores, 0.3, CrossConformalMethod.MinMax);

double lower = low;     // => 9.200000000000001
double upper = high;    // => 12.799999999999999
```

**Remarks** — MAPIE's `base` takes no member: it is
[`SplitConformal.Interval`](splitconformal-interval.md) around the model fitted on every sample,
at the scores' quantile. `naive`, which calibrates on the training samples, is not written.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`CrossConformal.Interval`](crossconformal-interval.md).
