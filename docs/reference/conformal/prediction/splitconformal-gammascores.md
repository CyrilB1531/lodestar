# SplitConformal.GammaScores

The gamma calibration scores of a regressor: `(y − ŷ) / ŷ`, for a strictly positive target.

<!-- docs-declaration -->

```csharp
public static double[] GammaScores(ReadOnlySpan<double> yTrue, ReadOnlySpan<double> yPredicted)
```

**Parameters** — `yTrue` are the observed values and `yPredicted` the model's predictions for
them, all strictly positive.

**Returns** — one signed score per point, in the input's order. Hand them to
[`GammaInterval`](splitconformal-gammainterval.md), or to
[`CrossConformal.GammaInterval`](crossconformal-gammainterval.md) when the predictions are out of
sample.

**Exceptions** — `ArgumentException` when the spans have different lengths;
`ArgumentOutOfRangeException` when a value or a prediction is zero, negative or `NaN`.

**Example** — a relative error, signed.

```csharp
using Lodestar.Conformal;

double[] scores = SplitConformal.GammaScores([10.2, 12.6], [10.7, 11.8]);

double under = scores[0];    // => -0.04672897196261683
double over = scores[1];     // => 0.06779661016949143
```

**Remarks** — MAPIE's `GammaConformityScore`: where the error grows with the target — prices,
durations, counts — a relative score gives an interval proportional to the prediction, and a
signed one lets the two sides differ. A non-positive value is refused, as MAPIE refuses it, since
the score's support is the positive reals.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SplitConformal.GammaInterval`](splitconformal-gammainterval.md),
[`SplitConformal.AbsoluteResiduals`](splitconformal-absoluteresiduals.md).
