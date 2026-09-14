---
status: accepted
supersedes: []
amends: []
applies: ["0081", "0095", "0099"]
---
# 0124 — The Cox model stays in Lodestar.Survival, and refuses what it cannot estimate

**Status:** accepted · **Date:** 2026-09-14 · **Applies:** [`0081`](0081-the-stats-numerical-layer-stays-internal.md), [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md), [`0099`](0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md)

## Context

[#684](https://github.com/CyrilB1531/lodestar/issues/684) asked for the Cox proportional hazards
model. `Lodestar.Survival` held Kaplan-Meier, Nelson-Aalen and the log-rank test: what survival looks
like, and whether two curves differ. It did not say by how much a covariate changes the hazard, which
is what a dataset with covariates is usually asked.
[Decision 0099](0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md)
found no .NET package for survival analysis and refused `scikit-survival` on its licence, so the
oracle is `lifelines`, as it is for the three members already shipped.

The specification (`docs/superpowers/specs/2026-09-12_0684_cox-proportional-hazards.md`) was
written before the code and corrected while writing it. Three of its measurements decide this
record.

## Decision

**[`CoxProportionalHazards.Fit`](../reference/survival/estimators/coxproportionalhazards-fit.md) ships in `Lodestar.Survival`**, returning `CoxSummary` and taking
`CoxOptions`. It fits by undamped Newton-Raphson from zero on the partial likelihood with Efron's
handling of ties, and reports coefficients, standard errors, z statistics, p-values, intervals,
hazard ratios, the log and null log partial likelihood, the likelihood-ratio test and Harrell's
concordance.

- **In this package, not a new one.**
  [Decision 0096](0096-ordinary-least-squares-earns-its-own-package.md) split OLS out of
  `Lodestar.Stats` because its audience wants none of the hypothesis tests. That argument does not
  transfer. A Cox model's caller holds the same censored durations a Kaplan-Meier curve is fitted on,
  and usually fits the curve first.
- **Nothing new is published from `Lodestar.Stats`.** The two tails the table needs,
  [`Distributions.NormalQuantile`](../reference/stats/tails/distributions-normalquantile.md) and
  [`Distributions.ChiSquaredSf`](../reference/stats/tails/distributions-chisquaredsf.md), were already public. Decision 0095
  does not fire again.
- **Efron ties only.** `CoxPHFitter` 0.30.3 takes no `ties` argument, so Breslow could be checked
  against nothing.
- **The corpus is lifelines at its maximum, compared at 1e-9.** At its defaults, lifelines stops when
  the Newton decrement falls below `precision=1e-7`. The decrement is quadratic in the gradient, so
  lifelines stops with the score near 1e-4 and the coefficients up to 8.6e-6 relative from the
  maximum. The generator fits with `precision=1e-20`, which brings all five fixtures within 3.1e-13
  of an independent Newton-Raphson. P-values stay relative at 1e-9, by 0081's rule.
- **Two designs are refused, where lifelines returns numbers behind a warning.**
  - **A collinear design** makes the observed information singular. It is refused when the
    Cholesky factor finds a pivot below 1e-12 of its diagonal.
  - **A separated design** has no maximum, yet it converges in floating point. The score underflows
    to exactly zero and the fit stops at a large, finite coefficient: on a five-subject design,
    β = 35.9 with a standard error of 2.9e7. That case is refused when the information at the
    converged step has fallen below 1e-10 of its value at zero. There it fell to 3e-16, where a
    strong finite effect on the same subjects kept about 0.1.
- **A pair with identical linear predictors counts one half in the concordance.** That is Harrell's
  rule and lifelines' stated rule. lifelines computes the partial hazard with row-dependent rounding,
  so two subjects with identical covariates can compare strictly there. On a ten-subject design it
  reported 33/42 where the rule gives 32.5/42.

## Options that lost

- **A `Lodestar.Survival.Regression` package**, on 0096's shape. Refused on audience, above.
- **Comparing coefficients relatively at 1e-7**, which the specification first proposed on the belief
  that lifelines discards its last step and that refitting from its own answer was the remedy.
  Measured again, refitting read `initial_point` in lifelines' standardised space and moved nothing,
  while tightening `precision` reached the maximum. The repository's tolerance is kept instead.
- **Returning lifelines' numbers for a separated or collinear fit, with a flag.** A table whose
  standard error is 2.9e7 reads as a result. An exception naming separation or collinearity tells the
  caller what to change in the design.
- **Reporting the iteration count.** lifelines reaches its answer under another stopping rule, so no
  corpus could check the count. A public member no oracle constrains can drift silently.

## Consequences

- `docs/equivalence.md` carries the `CoxPHFitter` rows and the three divergences above: the tightened
  stopping rule, the two refusals, and the half-counted tie.
- `docs/guides/survival-analysis.md` gains a Cox section that leads with the proportional-hazards
  assumption. This release does not test that assumption; Schoenfeld residuals and
  `proportional_hazard_test` are their own lot.
- Out of scope, each a decision of its own: prediction, the baseline hazard, strata, time-varying
  covariates, entry times, weights, penalisation, and cluster-robust variance.
