# Survival's review findings: Cox's step, the generalized gamma's shape derivatives, and the API names

**Issues:** [#1242](https://github.com/CyrilB1531/lodestar/issues/1242),
[#1221](https://github.com/CyrilB1531/lodestar/issues/1221),
[#1222](https://github.com/CyrilB1531/lodestar/issues/1222) and
[#1224](https://github.com/CyrilB1531/lodestar/issues/1224).
**Status:** written with the work, 2026-10-30.
**Date:** 2026-10-30.

## The problem

The pre-1.0 review of `Lodestar.Survival` left four issues:

- **#1242.** Cox's Newton fit took every step whole. On a 23-subject, two-covariate sample lifelines solves, the
  likelihood went −37.0, −73.4, −55.1, −306.8, and the fit was refused as separated.
- **#1222.** At `|λ|` past 31.6 the generalized gamma's shape `a = 1/λ²` fell below the `1e-3` step of its Richardson
  difference, and `ChiSquaredSf` threw on a negative df during a trial step.
- **#1221.** An event on a piecewise breakpoint took the left piece's hazard, where lifelines averages both.
- **#1224.** Several smaller points:
  - documentation that overstated parity;
  - `KaplanMeier.CompareAt` accepting a left-censored curve;
  - three API names out of line with the regression packages;
  - two fits re-summing every subject at every event time.

## Decisions

- **Cox halves a step that lowers the penalised likelihood**, by the search `ParametricNewton` uses, now one
  `NewtonSearch`. A fall within `1e-12` of the likelihood stands, so the last steps of a converging fit are unchanged.
  Where no halving keeps the likelihood, or halving leaves every coefficient where it was, the parametric fit converges
  only if the gradient has vanished, and Cox's is separated where the information collapsed, converged where Newton's
  whole step is below `1e-10` or its decrement below `1e-20` of the likelihood, and otherwise stalled, a refusal of its own.
- **The incomplete gamma is differentiated in `u = log x`.**
  - In x, the second derivative is of order `x^(a−2)`. At `x = 1e-169` and `a = 0.01` it overflowed before the chain
    rule's `x²` took it back, the Hessian read NaN, and Newton fell back to the gradient.
  - In u it is `du (a − x − du)`, finite throughout.
  - The Richardson step in `a` is `1e-3 a`: an absolute `1e-3` biased `d/da` by `2e-4` at `a = 0.007`.
  - Below `|λ| = 0.35` the likelihood follows Temme's uniform expansion about the log-normal, the one the quantile
    already inverts, with all twelve of `TemmeSeries`' rows; it meets the gamma's tail to `3e-13` there.
    - Past it the gamma's Richardson differences in `a = 1/λ²`, chained by `(2/λ³)²`, carry the tails' rounding into
      `H_λλ`: `9e-6` relative at `λ = 0.25` with a step of `1e-3 a`, `2e-6` at `0.35` with the second derivative's step
      tripled to `3e-3 a`, both for `z` in `[−5, 5]`; past `0.35` the expansion's own truncation passes `1e-13`.
    - Through the gamma at shape `1/λ²` the log density cancels terms of order `1/λ²`, its derivative in `λ` terms of
      order `1/λ³`: on 10,000 log-normal times Newton stalled at `λ = 9e-4`, and main threw on every log-normal sample
      tried.
    - The survival there is clipped to `[1e-35, 1 − 1e-35]` as the gamma's branch and lifelines clip it; unclipped, a
      subject 40 standard units out moved the likelihood by 446 across the expansion's edge.
    - An event's term is the log density, so the survival in `log h` and `H` is not computed only to cancel: the
      generalized gamma's and the log-normal's, univariate and AFT, are written in closed form.
    - At `λ = 0` exactly the survival is the log-normal's, unclipped, as lifelines takes it.
- **Where the generalized gamma's likelihood has no maximum, the fit throws** (Cyril, 2026-10-30).
  - On random samples the likelihood often rises along a ridge as `|λ|` grows.
  - lifelines returns where Nelder-Mead and then L-BFGS-B stopped, often short of any maximum.
  - A generalized gamma Newton step moving a parameter past three times its size, or three below one, is shortened to
    it, the other models' left whole, where a capped AFT fit took twice the steps:
    unbounded, the generalized gamma's first step reached a clipped region where the likelihood rises to `1e61`, and
    the fit threw on samples with one maximum; bounded, 381 of 400 random samples converged, against 377, and 380 once a spent budget refuses: the 381st crept
    along a ridge in λ near 21, flatter than the derivatives' noise, and lifelines stopped short of it at 19.6.
  - Reproducing that stopping point needs scipy's path bit for bit, which #1160 ruled out.
  - The divergence is documented in `docs/equivalence.md`.
- **Lodestar.Stats' normal tail keeps 3.5 ulps** (Cyril, 2026-10-30, fixed here): it read the generalized gamma's
  log-normal tail 15 ulps off. Its erfcx table was sampled at type initialization from a double continued fraction and
  indexed through y = 4/(4+x), whose position took 60ε; it is now fitted offline to a 70-digit erfc, 128 intervals of
  degree 8 in a variable linear in x, and z²'s rounding is restored in the exponent from the series' end, not from z = 6.
  `NormalSf` holds 3.5 ulps over `[−8, 38]`, against 59, at 10% more per call. Student's law from df `1e20` adds its
  first term in `1/df`, which the normal tail's old rounding hid: alone it read `StudentSf(37, 1e20)` 40 ulps off.
- **A parametric fit converges where Newton's decrement has vanished** to the likelihood's rounding, the gradient
  standing in only where the Hessian is not negative definite: the gradient ignores each parameter's scale, so a
  log-logistic AFT fit out of budget on a covariate of spread 1e-6 returned a coefficient of 192 for 501,641 as
  converged, and on times of spread 1e-5 of their size no double of μ met its bound at the maximum. A budget spent while
  the steps still move refuses, however small the decrement: its bound, 1e-10 of the likelihood, let three steps of a
  2,000-time log-normal fit stop 1.2e-6 from the maximum.
- **The AFT fits run on the design centred and scaled**, each covariate less its mean where its block has an
  intercept and over lifelines' deviation, the coefficients carried back as θ = Aφ and the covariance as A V Aᵀ: on
  raw columns 6 to 10 of 25 random fits per model, with covariates of spread 1e-6 to 1e6 or far from zero, did not
  converge where lifelines answers, and a covariate at 1e5 ± 0.04 moved the slope's standard error by 3e-3 through
  terms in x² that cancel; all now converge, and the slope's fit does not move with the covariate's offset.
- **Past the generalized gamma tail's clip at `1e-35` its derivatives are the clipped value's**, zero, as main's were:
  lifelines' `autograd_gamma` keeps the unclipped tail's own gradient there, which is not the derivative of the value it
  maximises; the equivalence row records the divergence.
- **Efron's risk sums are weighed from each risk set's own largest linear predictor**, rescaled as a larger one
  enters, not from the stratum's: past a spread of 745 a late risk set's sums underflowed, a design with a maximum
  at β = 1.509 read as separated, and a weighted separated one read a likelihood above its supremum and stalled. A
  budget spent on a collapsed information is a plateau the coefficient runs along, refused as separated.
- **The incomplete gamma takes its exponent whole** (Cyril, 2026-10-31): `a (log(1 + σ) − σ)` to some 106 bits,
  `log(x/a)` from a table and its first term in double-double, within 5.2e-24, less σ from its two-sum; Temme's erfc
  reads it, η from its root, and the series and the fraction their prefactor. The correct normal tail had uncovered
  the double path's own error, up to 872 ulps of a chi-squared tail far from its mean, and beside the region 2,794 of
  `Q(3000, 4800)`; the tails hold 2 ulps over 208 values in the region and 5 over 35 beside it to a = 3,000, the
  density with them. A tail read as one less the other keeps the double exponent, which it needs only to an ulp of one.
  Against scipy's vectorised `chdtrc` on varying arguments: `ChiSquaredSf(22000, 20000)` 78 ns to 84,
  `(2580, 2000)` 84 to 102, `(65.65, 101)` 67 to 68, `ChiSquaredCdf(1300, 2000)` 113 to 146, the density 29 to 36.
- **A parametric trial point whose derivatives overflowed fails its trial**, its value notwithstanding: a Weibull
  interval fit main solved took ρ to 1280, where (U/λ)^ρ overflowed, and every direction after read NaN.
- **A step under the tolerance ends a parametric fit only where Newton's decrement is under 1e-24 of the likelihood**,
  at a negative definite Hessian, and a parameter under a hundredth is shifted in its own scale as one past ten is:
  steps measured against one stopped an exponential scale of 1e-9 at 5e-9 of its closed form, and one of 1e-12 threw.
- **A fit whose every observation is a probability refuses a likelihood within 1e-9 of one**, their supremum, reached
  only by a point mass: twenty intervals sharing [1.19, 1.5] read a Weibull ρ of 783 at a standard error of 6e7 as a
  maximum.
- **#1221 was already fixed** by #1309, which averages the two pieces' hazards and froze the tie in the corpus.
- **A Breslow-Fleming-Harrington time no subject is at risk before its entrants adds no hazard.**
  - lifelines' smoothed increment indexes `cum_[−1]` there, the last entry.
  - This follows the estimator, not the wrap (decision 0010), and the row records the divergence.
- **`CompareAt` refuses a curve whose estimate is not the product of its steps' `1 − d/n`**, which is how a curve from
  `EstimateLeftCensored` reads. A positional record carries no flag, and its own constructor stays open. The bound is
  absolute, past both estimates' rounding: a relative one judged an all-observed left-censored curve, equal to the
  right-censored one, by its size.
- **API names** (Cyril, 2026-10-30):
  - `ParametricFit` becomes `ParametricSummary`;
  - `Aic` becomes `Akaike`;
  - the model selector follows the data, before the options, as `GeneralizedLinearModel.Fit` takes its family.
- **Aalen's risk sets are accumulated from the last event time back** (Cyril asked for both fits to be sped up).
  - Each row enters a sum once and none is subtracted, so no cancellation enters as the risk set shrinks.
  - One matrix is kept every √E event times, and a block's matrices are rebuilt from the next checkpoint, once per
    block where they fit within the checkpoints' budget.
  - Memory therefore stays at about `2√E d²`.
- **The time-varying likelihood reads each interval's risk once per evaluation.** Its intervals are sorted by stop,
  so an event time's scan ends at the first interval stopping before it. The risk set is still read per event time,
  as lifelines reads it; a sweep that subtracts leaving intervals was not taken, for the cancellation it brings.

## Settled

- A Cox search no halving completes, with the information not yet collapsed to `1e-10` of its value at zero, throws
  as stalled rather than separated: no separated design reached it in 900 random fits (Review A, pass 7).
- The second derivative in the shape keeps its own `3h` stencil: a `2h` one sharing `a ± h` saves two tail evaluations
  of nine but held `H_λλ` to `7e-6` at `|λ| = 0.35`, against `2e-6`.
- Cox's runaway rule reads Newton's whole step, as main's did, not the halved one taken: a large step on a flat
  likelihood is a runaway however far the search could follow it (Review A, passes 6 and 10).
- The likelihood's Stirling remainder keeps its own ten coefficients in λ², and `PowerSeries` its own Horner rule with
  curvature: Lodestar.Stats' Stirling terms are six, private to its gamma function, and its Temme evaluator is inlined
  on its own hot path; sharing either would move code this change does not otherwise touch.
- Cox's halving trials evaluate the score and information with the value: a step is halved only where Newton's whole
  one lowers the likelihood, rare once a fit converges, and a value-only path would be a second likelihood per model.
- The normal tail is monotone to its rounding, not to the ulp: where two of the table's intervals meet it may step an
  ulp against its slope, as rounding may inside one; two such steps in ±3000 doubles about every join.
- One left-censored generalized gamma case of the corpus carries a parameter tolerance of `5e-9`, beside the section's
  `2e-9`: the likelihood's rounding holds its λ to 4e-9 (Decisions), and the equivalence row says so.
- A Cox step past the doubles, from an overflowing score or information, is refused as singular, collinear at the first
  iteration, as main's NaN step reached that refusal at the next.
- Newton's path can climb a generalized gamma ridge that rises past a local maximum main or lifelines settles on, and
  the fit then throws as finding no maximum: 8 left-censored times of `rvA24`'s case 9 settle at λ = 0.175 there, 26.898,
  where the ridge towards λ = 1e6 reaches 27.316. Which side of such a sample a path takes depends on its steps.
- Where a Cox fit's linear predictors spread past about 709, Breslow's baseline overflows the doubles as lifelines'
  does: its cumulative hazard reads `+∞` there and a prediction from it `NaN`, as lifelines' reads. Main refused such
  designs as separated, wrongly; their coefficient table is exact.
- `CompareAt` tells a left-censored curve by its product, not by a flag on the record (Decisions).
- On a sample with several maxima the generalized gamma fit may stop on another one than lifelines, the log-normal's
  among them when a far subject's survival is clipped flat: documented, not matched.
- The generalized gamma's non-convergence where lifelines answers, on samples without a maximum near its start:
  documented, not matched (Decisions).
- `performance.md`'s Aalen and time-varying rows predate this change. The nightly benchmark re-measures them.
- Within `|λ| < 0.35`, a subject whose `|λζ|` passes the series' reach of 2.2 takes the gamma's Richardson differences,
  as every subject past `0.35` does, with the same `H_λλ` accuracy (Decisions); that far out its survival is under
  `1e-18` at `λ = 0.25`.
- A parametric fit whose decrement stays above `1e-24` of the likelihood under its derivatives' noise ends on the
  five-step stall rule, which costs up to five jets.
- The time-varying likelihood's shift scan reads one linear predictor a row before the accumulation's `p²` products a
  row; folding it into one pass would rescale the moments at every new largest instead.
- `NormalPdf` and `NormalSf` stay behind scipy's vectorised `norm._pdf` and `ndtr` as main's were, 4.2 ns to 2.3 and 12.2
  to 10.1: the exponent's residual adds 0.2 and 0.1 ns of it, where its rounding cost 19 ulps of the density.
