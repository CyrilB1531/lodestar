# SplitConformal.NormalisedResiduals

The normalised calibration scores of a regressor: `|y − ŷ| / r̂`.

<!-- docs-declaration -->

```csharp
public static double[] NormalisedResiduals(ReadOnlySpan<double> yTrue, ReadOnlySpan<double> yPredicted, ReadOnlySpan<double> residualEstimates)
```

**Parameters** — `yTrue` are the observed calibration values and `yPredicted` the model's
predictions for them. `residualEstimates` is a **second** model's prediction of `|y − ŷ|` at each of
those points, all strictly positive.

**Returns** — one score per calibration point, in the input's order. Hand them to
[`Quantile`](splitconformal-quantile.md).

**Exceptions** — `ArgumentException` when the three spans have different lengths;
`ArgumentOutOfRangeException` when an estimate is zero, negative or `NaN`.

**Example** — four calibration points whose spread the second model already knows differs.

```csharp
using Lodestar.Conformal;

double[] yTrue = [10.0, 12.0, 9.0, 15.0];
double[] yPredicted = [10.4, 11.0, 9.6, 13.5];
double[] residualEstimates = [0.5, 1.0, 0.5, 2.0];

double[] scores = SplitConformal.NormalisedResiduals(yTrue, yPredicted, residualEstimates);

double easy = scores[3];    // => 0.75
double hard = scores[2];    // => 1.1999999999999993
```

**Remarks** — this is MAPIE's `ResidualNormalisedScore`, and the reason to prefer it to
[`AbsoluteResiduals`](splitconformal-absoluteresiduals.md) is that the latter gives **every point
the same interval width**. On data whose error varies with the input — most data — that is too wide
where the model is confident and too narrow where it is not, while still carrying the marginal
coverage guarantee. The guarantee is what people come for, and it is exactly what makes a constant
width easy to mistake for an adequate one.

**Where `r̂` comes from is yours.** Fit a second regressor on `log |y − ŷ|` over data the first
model did not see, and exponentiate its prediction — the log is what keeps the estimate positive.
This package takes the estimate and not the model, which is the same choice every member here
makes: the caller owns the models, and what is written down is the arithmetic that carries the
guarantee.

**A non-positive estimate is refused rather than floored.** MAPIE thresholds at `1e-8`, because its
own residual model may predict a negative and it has nowhere to send the complaint. Here the
estimate is your argument, so flooring it would turn a bug into an interval of width `q · 1e-8` —
which reads as certainty. [Decision 0118](../../../decisions/0118-a-residual-estimate-is-refused-rather-than-floored.md)
has the divergence and why.

**The guarantee assumes exchangeability** — see the guide's
[*Exchangeability*](../../../guides/conformal.md#exchangeability) section.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SplitConformal.NormalisedInterval`](splitconformal-normalisedinterval.md),
[`SplitConformal.AbsoluteResiduals`](splitconformal-absoluteresiduals.md),
[`SplitConformal.Quantile`](splitconformal-quantile.md).
