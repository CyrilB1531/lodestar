# SplitConformal.NormalisedInterval

The prediction interval whose width varies with the input: `[ŷ − q·r̂, ŷ + q·r̂]`.

<!-- docs-declaration -->

```csharp
public static (double Lower, double Upper) NormalisedInterval(double prediction, double residualEstimate, double quantile)
```

**Parameters** — `prediction` is the model's point prediction for one new sample.
`residualEstimate` is the second model's `r̂` **at that same sample**, strictly positive. `quantile`
is the calibrated quantile from [`Quantile`](splitconformal-quantile.md) over
[`NormalisedResiduals`](splitconformal-normalisedresiduals.md).

**Returns** — a `(double Lower, double Upper)` tuple, in the target's own units.

**Exceptions** — `ArgumentOutOfRangeException` when `quantile` is negative or `NaN`, or when
`residualEstimate` is zero, negative or `NaN`.

**Example** — one quantile, two points, two widths.

```csharp
using Lodestar.Conformal;

double[] yTrue = [10.0, 12.0, 9.0, 15.0];
double[] yPredicted = [10.4, 11.0, 9.6, 13.5];
double[] residualEstimates = [0.5, 1.0, 0.5, 2.0];

double[] scores = SplitConformal.NormalisedResiduals(yTrue, yPredicted, residualEstimates);
double quantile = SplitConformal.Quantile(scores, alpha: 0.5);

(double Lower, double Upper) confident = SplitConformal.NormalisedInterval(11.0, 0.5, quantile);
(double Lower, double Upper) unsure = SplitConformal.NormalisedInterval(11.0, 2.0, quantile);

double tight = confident.Upper - confident.Lower;   // => 1
double wide = unsure.Upper - unsure.Lower;          // => 4
```

**Returns, read** — the same prediction and the same quantile, four times the width, because the
second model says one point is four times as hard as the other. That is the whole difference from
[`Interval`](splitconformal-interval.md), which would have given both the same.

**Remarks** — an infinite `quantile` yields the whole line, as
[`Interval`](splitconformal-interval.md) does: the multiplication carries the infinity through
rather than turning it into `NaN`, which is what
[decision 0070](../../../decisions/0070-k-greater-than-n-returns-an-infinite-interval.md) needs to
hold here too.

Pass the estimate for **the point being predicted**, not the calibration mean. Passing a constant
reduces this to [`Interval`](splitconformal-interval.md) with the quantile rescaled, which is a
valid thing to do and not what the score is for.

**The guarantee assumes exchangeability** — see the guide's
[*Exchangeability*](../../../guides/conformal.md#exchangeability) section.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SplitConformal.NormalisedResiduals`](splitconformal-normalisedresiduals.md),
[`SplitConformal.Interval`](splitconformal-interval.md).
