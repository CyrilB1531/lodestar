# Stratified, weighted, penalised and robust Cox, the proportional hazards test and time-varying Cox

**Issue:** [#1171](https://github.com/CyrilB1531/lodestar/issues/1171).
**Status:** written with the work, 2026-09-26.
**Date:** 2026-09-26.

## The problem

`docs/equivalence.md` carried lifelines' stratified, penalised, robust and clustered Cox fits, the
proportional hazards test and `CoxTimeVaryingFitter` as *to write for 1.0*, and the L1 penalty as
*not written*. The issue added `weights_col`, the baselines and lifelines' predictions to the scope.

## What lifelines 0.30.3 does

- `CoxPHFitter` sorts the sample by stratum, duration and event with a stable multi-key sort,
  standardises each covariate by its mean and sample deviation, runs its Newton loop on that scale and
  rescales. The penalty `n · λ · Σ (l1 · softabs(β, 1.3^i) + (1 − l1) β²/2)` is on the standardised
  coefficients; `log_likelihood_` is the penalised value. Weighted Efron multiplies each time's terms by
  the mean weight of its events. Strata sum their likelihoods; the concordance sums pairs per stratum.
- The robust variance is `DᵀD`, `D` the row-level score residuals (ties ignored, so order matters)
  times the inverse information over the deviations, summed per cluster; clusters force it.
- The baseline is Breslow's per stratum at every distinct duration, zero where a stratum has no event,
  centred on the means. Predictions interpolate the cumulative hazard linearly (`numpy.interp`); the
  expectation is a trapezoid over the observed span.
- `proportional_hazard_test` scales the Schoenfeld residuals of the raw covariates at the raw
  coefficients by the number of events and the model covariance, and divides by the fit's own
  standard errors. Its Kaplan-Meier transform is lifelines' weighted estimator: Kahan-summed weights,
  a log-space cumulative sum that skips a term whose risk set rounds below the deaths.
- `CoxTimeVaryingFitter` builds each event time's risk set from the intervals spanning it; its loop
  sharpens by `1.5^i`, applies each step after its tests and returns the stepped point; `robust=True`
  raises `NotImplementedError`; its baseline pools the strata and divides weighted events by
  unweighted hazards.

## Decisions

As approved on the issue, point 8 widened to include the L1 penalty, the weights and the predictions:

1. `CoxProportionalHazards.Fit(design, durations, events, weights, strata, clusters, featureCount, options)`.
2. `CoxOptions.Penalizer`, `L1Ratio` and `Robust`.
3. Lifelines' arithmetic as it is, standardisation included.
4. The sandwich from lifelines' row-level residuals, clusters summed.
5. Strata summed, the concordance per stratum.
6. `TestProportionalHazards` with `CoxTimeTransform { Rank, KaplanMeier, Identity, Log }`.
7. `CoxTimeVarying.Fit`, with weights and strata; robust refused, as lifelines implements none.
8. The L1 penalty by lifelines' own loop, reproduced step for step: an ulp of noise moved lifelines'
   answer by `8e-16` on twelve random fits, so the loop is a parity target. The baselines
   (`CoxBaseline`) and the predictions on `CoxSummary`.
9. Proof by a corpus, a random differential and a benchmark.

## The one divergence

lifelines computes the partial hazards with a BLAS `dot` whose row blocking can leave two subjects
with identical covariates one ulp apart, so Harrell's index counts them untied; this counts them tied.
`coxsummary.md` already documented it for the unweighted fit.

## Proof

- `survival_cox_extended.json`, 64 cases from lifelines 0.30.3 at `1e-9`: 33 fits over three designs
  and eleven settings, 21 proportional hazards tests under all four transforms, and 10 time-varying
  fits, each with its baselines and predictions.
- A random differential against lifelines: 422 fits and tests under two seeds and 80 time-varying
  fits. Two fits differ, both on the concordance, both from the BLAS artifact above.
- A benchmark against lifelines at 1,000 and 10,000 subjects, ahead on every row: 37× to 126× on the
  fits, the test and the predictions, and 1.79× on the time-varying fit, whose risk sets both sides
  rebuild at every event time.
