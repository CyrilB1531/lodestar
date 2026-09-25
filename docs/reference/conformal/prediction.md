# Conformal prediction — `Lodestar.Conformal`

A model gives you one number, or one class. This page turns it into an **interval**, or a **set**,
that contains the truth a stated fraction of the time — 90 % of the time, say — and the fraction is
a finite-sample guarantee rather than an asymptotic hope. It costs a held-out calibration set and
nothing else: no retraining, no distributional assumption, and no assumption that the model is any
good. A bad model gets wide intervals, which is the correct answer.

For split conformal there is one type, [`SplitConformal`](prediction/splitconformal.md), and it is static, beside the
[`ConformalQuantileRule`](prediction/conformalquantilerule.md) its quantile takes. The
calibrated quantile is handed back to you rather than kept inside an object, because it is the
number that carries the guarantee and you should be able to look at it.

The whole procedure is three calls:

1. Score the calibration set —
   [`AbsoluteResiduals`](prediction/splitconformal-absoluteresiduals.md) for a regressor,
   [`LeastAmbiguousScores`](prediction/splitconformal-leastambiguousscores.md) for a classifier.
2. Turn the scores into one number — [`Quantile`](prediction/splitconformal-quantile.md).
3. Apply it to a new prediction — [`Interval`](prediction/splitconformal-interval.md) or
   [`PredictionSet`](prediction/splitconformal-predictionset.md).

> **The guarantee assumes exchangeability.** It does not hold for time series, for data with drift,
> or for any split that leaks. The intervals still come out; they simply do not cover, and nothing
> in the output says so. The guide's
> [*Exchangeability*](../../guides/conformal.md#exchangeability) section is the part of this
> documentation worth reading before the API.

| Member | What it does |
| --- | --- |
| [`SplitConformal.Quantile`](prediction/splitconformal-quantile.md) | The calibrated quantile: the `k`-th smallest score, with `k = ceil((n + 1)(1 − α))`. |
| [`ConformalQuantileRule`](prediction/conformalquantilerule.md) | Which order statistic `Quantile` reads: the ceiling rank, or MAPIE's classification quantile. |
| [`SplitConformal.AbsoluteResiduals`](prediction/splitconformal-absoluteresiduals.md) | A regressor's calibration scores, `\|y − ŷ\|`. |
| [`SplitConformal.Interval`](prediction/splitconformal-interval.md) | `[ŷ − q, ŷ + q]` around a point prediction. |
| [`SplitConformal.NormalisedResiduals`](prediction/splitconformal-normalisedresiduals.md) | A regressor's scores divided by a predicted residual, `\|y − ŷ\| / r̂`. |
| [`SplitConformal.NormalisedInterval`](prediction/splitconformal-normalisedinterval.md) | `[ŷ − q·r̂, ŷ + q·r̂]`, whose width varies with the input. |
| [`SplitConformal.GammaScores`](prediction/splitconformal-gammascores.md) | A regressor's signed relative errors, `(y − ŷ) / ŷ`, for a positive target. |
| [`SplitConformal.GammaInterval`](prediction/splitconformal-gammainterval.md) | `[ŷ(1 + q_low), ŷ(1 + q_up)]`, one quantile a side. |
| [`SplitConformal.LeastAmbiguousScores`](prediction/splitconformal-leastambiguousscores.md) | A classifier's LAC calibration scores, `1 − p̂(true class)`. |
| [`SplitConformal.PredictionSet`](prediction/splitconformal-predictionset.md) | Every class whose probability clears `1 − q`. Possibly none. |

## Without a calibration set

When the data is too scarce to hold a calibration set out,
[`CrossConformal`](prediction/crossconformal.md) scores every training sample with a model fitted
without it — CV+, Jackknife+ and the jackknife-after-bootstrap — at the price of a weaker
guarantee, `1 − 2α`.

| Member | What it does |
| --- | --- |
| [`CrossConformal.OutOfSample`](prediction/crossconformal-outofsample.md) | Each training sample's prediction by the models fitted without it. |
| [`CrossConformal.Interval`](prediction/crossconformal-interval.md) | The interval at a test point for the absolute-residual score. |
| [`CrossConformal.GammaInterval`](prediction/crossconformal-gammainterval.md) | The interval at a test point for the gamma score. |
| [`CrossConformalMethod`](prediction/crossconformalmethod.md) | Plus or min-max. |
| [`CrossConformalAggregation`](prediction/crossconformalaggregation.md) | Mean or median, over the models that held a sample out. |
