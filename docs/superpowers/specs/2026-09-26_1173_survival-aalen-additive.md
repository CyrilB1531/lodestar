# Aalen's additive hazards model

**Issue:** [#1173](https://github.com/CyrilB1531/lodestar/issues/1173).
**Status:** written with the work, 2026-09-26.
**Date:** 2026-09-26.

## The problem

`docs/equivalence.md` carried lifelines' `AalenAdditiveFitter` as *to write for 1.0*: the cumulative
regression coefficients, their variance and the predictions, a ridge-penalised least squares at each
event time with no optimiser to pin.

## What lifelines 0.30.3 does

- It sorts the subjects by duration, scales each covariate by its pandas sample deviation (the
  intercept, added last, by one) and multiplies each row by the square root of its weight.
- At each distinct event time it solves `(XᵀX + (c₁ + c₂)I) v = Xᵀy + c₂ v_prev` by a Cholesky solve
  (`assume_a="pos"`), `y` the deaths there; the variance of the increments is the squared columns of
  the same solve over the deaths, summed. A system that is not positive definite gives zero increments.
- It then zeroes the rows of the subjects whose duration is that event time, and stops once
  `3(d − 1)` is at least the subjects not yet removed.
- It divides the increments and their cumulative sums by the deviations, **and the cumulative
  variance by the deviations too**, not their squares.
- A subject censored between two event times is never removed: it stays at risk to the end.
- `summary` regresses each cumulative coefficient through the origin on time, weighted by the true
  number at risk; `concordance_index_` pairs the sorted durations with predictions made in the order
  given; `predict_percentile` is numpy's `searchsorted` on the survival, which need not be monotone.

## Decisions

As approved on the issue, option A: every reading above is reproduced, defects included, so the
numbers are lifelines'. `AalenAdditive.Fit(design, durations, events, [weights], featureCount,
AalenOptions)` returns `AalenSummary`: the increments, cumulative coefficients, variance, bounds,
slopes table, concordance, the five predictions and the smoothed hazards. Three places part from it:

- With tied durations, lifelines' concordance pairing follows numpy's unstable sort, which the CPU
  decides; this sorts stably.
- A covariate constant beside the intercept (every value the same) raises `ArgumentException`,
  where lifelines divides by a zero deviation.
- Without a penalty, a singular system gives zero increments, lifelines' `LinAlgError` answer, wherever
  the Cholesky pivot is within 8 ulps of its diagonal per subject at risk: the rounding a singular pivot keeps grows with
  the sums behind it, measured at 6 ulps for 50 subjects, 15 for 1,000 and 76 for 20,000. A
  covariate constant among the subjects left at risk makes the system singular in exact arithmetic,
  and LAPACK's pivot then lands either side of zero by its
  summation order: in the corpus's singular tail it fails at months 15 and 18 unweighted, but weighted
  solves at month 15 (a pivot of `+2.2e-16` of its diagonal) and fails at 18. That weighted fit is left
  out of the corpus; the code review found the case in the reference pages' own example.

numpy's `searchsorted` was matched by measurement: it halves a window from its base rather than
bisecting a range, and the two land apart on a curve that is not monotone. The window form agreed
with numpy on 20,000 random arrays out of 20,000, on every SIMD tier; the classic form on 18,696.

## Proof

- `survival_aalen.json`, 27 fits over four samples (tie-free, tied, few subjects, and a singular tail)
  and seven settings (default, no intercept, each penalty, both, weights, a level of 0.9) at `1e-9`;
  the concordance is compared where durations do not tie.
- A random differential against lifelines, 150 fits of random size, penalties, weights, ties and
  intercept: all agree at `1e-9`.
- A benchmark against lifelines at 1,000 and 10,000 subjects, ahead on every row in processor time:
  55× and 10× on the fits, 1.6× and 1.9× on the predictions; on the elapsed clock lifelines' BLAS
  threads put its prediction at 10,000 ahead, 1.14 ms against 2.21.
