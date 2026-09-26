# Parametric survival: univariate fits, accelerated failure time, left and interval censoring

**Issue:** [#1172](https://github.com/CyrilB1531/lodestar/issues/1172).
**Status:** written with the work, 2026-09-26.
**Date:** 2026-09-26.

## The problem

`docs/equivalence.md` carried lifelines' parametric univariate fitters, `BreslowFlemingHarringtonFitter`,
the three AFT fitters, their left and interval censoring and `KaplanMeierFitter.fit_left_censoring`
as *to write for 1.0*. The review widened the scope to the generalized gamma, which #1160 had left
*not written* at a `1.6e-9` pin, and moved `SplineFitter` and the AFT L1 penalty to
[#1184](https://github.com/CyrilB1531/lodestar/issues/1184).

## What lifelines 0.30.3 does

- Each univariate fitter minimises the mean negative log-likelihood by Nelder-Mead and then
  L-BFGS-B, which stop `1e-6` to `1e-3` short of the maximum. `log_likelihood_` is the weighted sum;
  right-censored `Σ w (e log h − H)`, left-censored `Σ w (e (log h − H) + (1 − e) log(1 − S))`,
  interval-censored an event where the bounds coincide and `log clip(S(L) − S(U), 1e-25, 1 − 1e-25)`
  otherwise, bounds clipped to `[1e-20, 1e25]`; each entry adds `w H(entry)`.
- Its variance is the inverse autograd Hessian; each z statistic is measured from
  `_compare_to_values`; the bands are the delta method on `S` and `H` themselves.
- `GeneralizedGammaFitter` differentiates the regularised incomplete gamma in its shape by finite
  differences (`autograd_gamma`), so its gradient stops near `3e-11` and its standard errors are up to
  `2e-5` relative off.
- The AFT fitters fit on the covariates divided by their sample deviations, starting from the
  univariate fit's parameters logged into the intercepts; the ridge penalty `λ Σ β²/2` spares a
  constant column only in a block of more than one; the variance is the inverse Hessian of the summed
  objective rescaled; the sandwich differentiates each subject's unweighted term plus the penalty at the
  unscaled coefficients; the null model is the univariate fit; the concordance reads the predicted
  medians, and raises `AttributeError` for interval censoring.
- `LogLogisticAFTFitter.__init__` drops `model_ancillary`.
- `BreslowFlemingHarringtonFitter` is `exp(−H)` of a smoothed Nelson-Aalen fit, without weights; its
  entrants are kept out of the risk set of their own time's events except at the first row. The
  left-censored Kaplan-Meier reverses the time axis. Both label the larger bound their lower one.

## Decisions

As approved on the issue:

1. `ParametricSurvival.Fit`, `FitLeftCensored`, `FitIntervalCensored`, with weights and entries, over
   `ParametricModel { Exponential, Weibull, LogNormal, LogLogistic, PiecewiseExponential,
   GeneralizedGamma }`, returning `ParametricFit`; `ParametricOptions` carries the level, the budget and
   the breakpoints.
2. Newton to the maximum on exact derivatives — a second-order forward-differentiation jet — with
   bound-respecting halving and a Levenberg-Marquardt shift; the shape derivatives of the incomplete
   gamma by Richardson-extrapolated central differences.
3. `AcceleratedFailureTime.Fit`, `FitLeftCensored`, `FitIntervalCensored` over
   `AftModel { Weibull, LogNormal, LogLogistic }`, returning `AftSummary` with lifelines' predictions;
   `AftOptions` carries `FitIntercept`, `Ancillary`, the ridge `Penalizer` and `Robust`. The fit runs on
   the unscaled coefficients with the penalty rescaled, which has the same optimum and variance.
4. `BreslowFlemingHarrington.Estimate` returning `SurvivalCurve`, and `KaplanMeier.EstimateLeftCensored`.
5. `ParametricSurvival.CompareAt`, the parametric form of the fixed-point test #1170 deferred here.
6. `CRCSplineFitter` stays *not written*: an ulp in the data moves its fit by `1.8e-5`.
7. The new data types go to `Lodestar.Abstractions`; `Lodestar.Survival` reaches it and
   `Lodestar.Stats` by project until the next publication, for the tails the fits read.

## The divergences

- The fits are at the maximum, where lifelines' defaults stop short.
- The generalized gamma's standard errors come from an independent value-only Hessian in the corpus.
- `AftOptions.Ancillary` models the log-logistic shape, which lifelines' constructor drops.
- `SurvivalCurve.Lower` and `KaplanMeierCurve.Lower` are the smaller bound.
- `AftSummary.ConcordanceIndex` is `NaN` for interval censoring rather than an exception.

## Proof

- `survival_parametric.json`, 155 cases from lifelines 0.30.3: 81 univariate fits over two samples,
  three censorings, weights and entry at `1e-9`, the generalized gamma at `2e-9` — its four
  interval-censored fits at `1e-6` and their inference at `1e-5`, polished on values alone where autograd's shape derivative is `NaN`; 48 AFT fits over
  every censoring and option at `1e-9`; 12 Breslow-Fleming-Harrington and left-censored Kaplan-Meier
  curves at `1e-12`; 14 fixed-point tests.
- A random differential against lifelines: 377 cases over four seeds. All AFT fits and curves agree at
  `1e-9`; two generalized gamma fits with `λ` near zero part by `5e-8` on a parameter and `4e-6` on a
  standard error, where lifelines' own gradient stops at `7e-9`.
- A benchmark against lifelines at 1,000 and 10,000 subjects, ahead on every row: 9.95× to 283× at 1,000,
  1.44× to 207× at 10,000. The first run had the Weibull regression at 0.83× at 10,000, its jets carrying
  every coefficient; differentiating each subject in its two predictors alone took it to 1.44×.
