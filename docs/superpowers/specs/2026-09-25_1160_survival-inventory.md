# Lodestar.Survival against lifelines, read before 1.0.0

**Issue:** [#1160](https://github.com/CyrilB1531/lodestar/issues/1160).
**Status:** written with the work, 2026-09-25.
**Date:** 2026-09-25.

## The problem

`Lodestar.Survival` ships Kaplan-Meier, Nelson-Aalen, the log-rank test and the Cox model, and
`docs/equivalence.md` carried no *not written* row for it. The table listed what was written, and
lifelines 0.30.3 publishes a good deal more; without an inventory, the absence of gap rows read as a
completeness it had not earned.

## How each fit was judged

[Decision 0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md)'s test: a fit
is writable when its reference pins its own answer at the `1e-9` a corpus is compared at. For each
optimiser-based fitter, on three simulated right-censored samples of 300 (Weibull times, covariate
effects, exponential censoring), `scipy.optimize.minimize` inside lifelines was wrapped so that its
result was polished by Newton on lifelines' own gradient, from the default fit and from a tight one
(`gtol=1e-14`, `ftol=1e-16`). Two optima that agree to `1e-9` with a vanishing gradient are one
closed optimum a C# Newton reaches too; lifelines' default stopping point is then a documented
divergence, as the Cox row already records. Cox fits have their own Newton, so it was rerun at full
step from the default and from the tight fit (`precision=1e-20`), its `initial_point` scaled by each
column's standard deviation because lifelines reads it in its normalised space; Turnbull's EM was
run at three tolerances.

## Measured

| fit | default → optimum | optimum from default ↔ from tight | verdict |
| --- | ---: | ---: | --- |
| Exponential, Weibull, LogNormal, LogLogistic, PiecewiseExponential | `1.2e-6` – `6.8e-6` | `≤ 8.2e-16` | writable |
| Weibull, LogNormal, LogLogistic AFT | `8.1e-6`, `2.0e-4`, `1.3e-3` | `≤ 6.2e-14` | writable |
| the five univariate fitters, left and interval censored | `6.0e-11` – `1.4e-5` | `≤ 2.9e-15` | writable |
| GeneralizedGamma | `2.5e-4` | `1.6e-9`, gradient `4.9e-11` | not written |
| Spline | — | Newton leaves the domain; tight fit does not converge | not written |
| CRCSpline | 35 % – 1,900 % | or a singular Hessian | not written |
| Cox, strata / L2 penalty / robust / cluster | `6.2e-7` – `3.2e-6` | `≤ 2.4e-14`, standard errors `≤ 5.1e-16` | writable |
| Cox, L1 penalty | factor 29 | — | not written |
| Cox, time-varying | `1.8e-16` | `9.8e-14` | writable |
| Turnbull EM | `9.5e-3` at `tol=1e-5` | `3.1e-5` between `1e-10` and `1e-13` | not written |

The log-rank weightings, the multivariate and pairwise tests, the fixed-point survival difference,
the restricted mean survival time, the concordance index, Breslow-Fleming-Harrington, the
left-censored Kaplan-Meier and Aalen's additive model are closed forms and need no measurement.

## Decisions

1. **To write for 1.0**, as Cyril decided, in four issues: the tests and statistics
   ([#1170](https://github.com/CyrilB1531/lodestar/issues/1170)); Cox's strata, L2 penalty, robust
   errors, proportional-hazards test and time-varying fit
   ([#1171](https://github.com/CyrilB1531/lodestar/issues/1171)); the parametric and AFT fitters
   with left and interval censoring ([#1172](https://github.com/CyrilB1531/lodestar/issues/1172));
   Aalen's additive model ([#1173](https://github.com/CyrilB1531/lodestar/issues/1173)).
2. **Not written**, each with its measured reason: GeneralizedGamma, the two spline fitters, the
   L1-penalised Cox and Turnbull's estimator.
3. **The corpora of #1171 and #1172 hold the Newton-polished optimum**, not lifelines' default
   stopping point, as the present Cox corpus holds its tight fit.

## Proof

The rows in `docs/equivalence.md`, one per part of the surface #1160 listed; the scripts that
measured them are not committed, and the numbers above are what they printed.
