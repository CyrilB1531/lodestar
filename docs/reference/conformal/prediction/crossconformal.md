# CrossConformal

Cross-conformal regression: CV+, Jackknife+ and the jackknife-after-bootstrap, at MAPIE parity,
with no calibration set held out.

<!-- docs-declaration -->

```csharp
public static class CrossConformal
```

**Example** — CV+ over three folds, at 70 % coverage.

```csharp
using Lodestar.Conformal;

// Six training samples in three folds; model m was fitted without fold m.
int[] folds = [0, 0, 1, 1, 2, 2];
double[] yTrue = [10.2, 11.5, 9.8, 12.6, 11.1, 10.4];
double[] outOfFold = [10.7, 10.5, 10.0, 11.8, 11.4, 11.0];   // each sample's prediction by its fold's model
double[] atTest = [10.0, 11.0, 12.0];                        // the three models' predictions at a new point

double[] scores = SplitConformal.AbsoluteResiduals(yTrue, outOfFold);
(double lower, double upper) = CrossConformal.Interval(atTest, folds, scores, alpha: 0.3);

double low = lower;     // => 9.5
double high = upper;    // => 12.3
```

**Remarks** — [`SplitConformal`](splitconformal.md) spends a calibration set the model never
sees. This class spends nothing: every training sample is scored by a model fitted without it, and
a new point's interval is built from every model's prediction there. Model `m` of `M` is fitted
without the samples its column of a **held-out mask** marks — one fold each for CV+, one sample
each for Jackknife+, a bootstrap's out-of-bag samples for the jackknife-after-bootstrap. As
everywhere in this package the models are yours: this takes their predictions.

**The guarantee is `1 − 2α`** for CV+ and Jackknife+ (Barber et al., 2021), not the `1 − α` of
split conformal, though coverage near `1 − α` is what is usually observed. It assumes
exchangeability — see the guide's [*Exchangeability*](../../../guides/conformal.md#exchangeability)
section.

MAPIE's `method="base"` is the split interval over the out-of-sample scores:
[`SplitConformal.Interval`](splitconformal-interval.md) around the model fitted on every sample,
at [`SplitConformal.Quantile`](splitconformal-quantile.md) of the scores, or
[`SplitConformal.GammaInterval`](splitconformal-gammainterval.md) for the gamma score.

**Applies to** — net10.0, netstandard2.0.

**See also** — the [conformal index](../prediction.md), [`SplitConformal`](splitconformal.md).

## Members

| Member | What it does |
| --- | --- |
| [`CrossConformal.OutOfSample`](crossconformal-outofsample.md) | Each training sample's prediction by the models fitted without it. |
| [`CrossConformal.Interval`](crossconformal-interval.md) | The interval at a test point for the absolute-residual score. |
| [`CrossConformal.GammaInterval`](crossconformal-gammainterval.md) | The interval at a test point for the gamma score. |
| [`CrossConformalMethod`](crossconformalmethod.md) | Plus or min-max. |
| [`CrossConformalAggregation`](crossconformalaggregation.md) | Mean or median, over the models that held a sample out. |
