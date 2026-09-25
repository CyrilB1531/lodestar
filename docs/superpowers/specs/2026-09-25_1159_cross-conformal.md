# Cross-conformal regression, CV+ and Jackknife+, and the gamma conformity score, at MAPIE parity

**Issue:** [#1159](https://github.com/CyrilB1531/lodestar/issues/1159).
**Status:** written before the work, 2026-09-25.
**Date:** 2026-09-25.

## The problem

`docs/equivalence.md` carries `GammaConformityScore` and MAPIE's CV and Jackknife+ regressors as
*no counterpart*. Split conformal spends a calibration set; CV+ and Jackknife+ are what MAPIE
recommends when the data is too scarce to hold one out, and the gamma score is its multiplicative
score for a strictly positive target.

## What MAPIE 1.5.0 computes

Read from `mapie.estimator.regressor.EnsembleRegressor`, `mapie.conformity_scores.regression` and
`mapie.utils._compute_regression_quantile`, and checked by an independent numpy reading against
`CrossConformalRegressor` and `JackknifeAfterBootstrapRegressor` on 255 random problems before any
C#:

- **Out-of-sample predictions.** Model `m` is fitted without the samples its mask `k_[:, m]` marks.
  Sample `i`'s out-of-sample prediction is the mean, or the median, of the models that exclude it:
  one model for K-fold (CV+) and leave-one-out (Jackknife+), several for the jackknife-after-
  bootstrap. Its score is `|yᵢ − ŷ₋ᵢ|`, or `(yᵢ − ŷ₋ᵢ)/ŷ₋ᵢ` for gamma.
- **A test point `x`** gets one prediction per training sample, `ŷ₋ᵢ(x)`: the same aggregate of
  the models that exclude `i`, evaluated at `x`.
- **`plus`**: the bounds are quantiles, over `i`, of `ŷ₋ᵢ(x) − Rᵢ` and `ŷ₋ᵢ(x) + Rᵢ` (absolute), or
  of `ŷ₋ᵢ(x)(1 + Rᵢ)` (gamma).
- **`minmax`**: `min ŷ₋ᵢ(x) − q` and `max ŷ₋ᵢ(x) + q`, `q` the scores' quantile; for gamma,
  `min ŷ₋ᵢ(x)(1 + q_low)` and `max ŷ₋ᵢ(x)(1 + q_up)`.
- **`base`**: the model fitted on every sample, `ŷ(x) ± q`: the split interval over the
  out-of-sample scores.
- **The quantile** is the ceiling rank `k = ⌈α_ref(n + 1)⌉`, read by numpy's `lower` method at
  level `k/n`. The absolute score reads `α` below and `1 − α` above; gamma, which is not
  symmetric, reads `α/2` below and `1 − α/2` above. **The lower side's level is evaluated as
  `(1 − 2a) + a`**, which is not always `1 − a` in floating point (`0.9 + 0.05` is
  `0.9500000000000001`), so the rank moves by one there — at `n = 59` and `119` for `a = 0.05`, measured over `n < 200`; it is replayed as written.

## Decisions

1. **One static class, `CrossConformal`**, taking predictions rather than a model, as
   `SplitConformal` does. The caller hands over each model's predictions and a held-out mask,
   `bool[n × M]` row-major, `true` where model `m` was fitted without sample `i`:
   - `OutOfSample(predictions, heldOut, modelCount, aggregation)` — each training sample's
     out-of-sample prediction from the `n × M` block of every model's predictions on the training
     samples;
   - `Interval(testPredictions, heldOut, scores, alpha, method, aggregation)` for the absolute
     score and `GammaInterval(…)` for gamma — one test point, its `M` model predictions;
   - overloads taking one fold index per sample instead of the mask, for CV+ and Jackknife+,
     where each sample is held out by exactly one model.
   Scores come from `SplitConformal.AbsoluteResiduals` and the new `SplitConformal.GammaScores`.
2. **`CrossConformalMethod { Plus, MinMax }`** and **`CrossConformalAggregation { Mean, Median }`**,
   in `Lodestar.Abstractions` under `Lodestar.Conformal`, forwarded as `ConformalQuantileRule` is.
   **`base` takes no member**: it is `SplitConformal.Interval(ŷ(x), SplitConformal.Quantile(scores,
   α))` over the out-of-sample scores, and `SplitConformal.GammaInterval` for gamma, which the corpus
   proves against MAPIE's own `method="base"`. A member would need the full model's prediction, an
   input no other member takes. `naive` calibrates on the training samples and is left out.
3. **The jackknife-after-bootstrap is included** through the same mask; its oracle reads MAPIE's
   fitted `k_` and each model's predictions, so MAPIE's resampling is replayed rather than seeded.
4. **The gamma score on the split path too**: `SplitConformal.GammaScores(yTrue, yPredicted)` and
   `SplitConformal.GammaInterval(prediction, scores, alpha)`, which takes the scores rather than a
   quantile because an asymmetric score needs two. A target or a prediction at or below zero is
   refused, as MAPIE refuses it.
5. **`minimize_interval_width` is not written**: it applies to asymmetric scores only and optimises
   `β` by a scan; a row in `docs/equivalence.md` says so.
6. **A rank past `n` is an infinite bound**, as decision 0007 settles for the split path; MAPIE
   raises there too, when `1/α` or `1/(1 − α)` is not below the sample count.
7. **One divergence, a reference defect**: a sample no model excludes has no out-of-sample
   prediction. MAPIE skips it in the scores and under `plus`, through `nanquantile`, but its `minmax`
   takes `numpy.min` over a column holding `NaN` and returns `NaN` bounds for every test point
   (measured: every one of the 10 `minmax` problems with such a sample, and none of the 36
   without). Here the sample is dropped by every method.
8. **Refused**: `alpha` outside `(0, 1)`, a mask whose length is not `n × M`, `M < 1`, a fold index
   outside `[0, M)`, mismatched lengths, a `NaN` score on a sample some model excludes, an
   undeclared method or aggregation, and for gamma a non-positive target or prediction.

## Proof

- `tests/oracles/conformal_cross.json` from MAPIE 1.5.0: `CrossConformalRegressor` at K-fold and
  leave-one-out, `JackknifeAfterBootstrapRegressor` at mean and median aggregation, each under
  `plus`, `minmax` and `base`, with the absolute and the gamma score; the split regressor with the
  gamma score: 146 cases, 44 K-fold and leave-one-out under plus and minmax, 22 under base, 74
  jackknife-after-bootstrap (14 with a sample in every bag), 6 split gamma. Bounds and scores
  compared at `1e-9` relative, since a mean over several models sums in another order.
- A random differential against MAPIE: 229 cross-conformal problems of 10 to 80 samples at seven
  levels, and 80 split gamma problems, every bound within the same tolerance.

## Knock-on changes

Reference pages, `docs/equivalence.md` rows replacing the *no counterpart* one, the conformal guide,
a sample, the changelogs, and a benchmark against MAPIE in the package's `performance.md`.
