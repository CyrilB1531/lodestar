# The log-rank family, the restricted mean, the fixed-point test and the concordance index

**Issue:** [#1170](https://github.com/CyrilB1531/lodestar/issues/1170).
**Status:** written before the work, 2026-09-26.
**Date:** 2026-09-26.

## The problem

The inventory of #1160 left five `docs/equivalence.md` rows as *to write for 1.0* against
`Lodestar.Survival`: the weighted log-rank tests, the multi-group and pairwise forms, the fixed-point
comparison of two curves, the restricted mean survival time and the concordance index on any score.
Each is a closed form over the step table `KaplanMeier` and `LogRank` already build.

## What lifelines 0.30.3 does

- `multivariate_logrank_test` carries every form: `logrank_test` calls it with two groups, and
  `pairwise_logrank_test` calls `logrank_test` per pair of labels, sorted. At each distinct duration it
  sets each group's weighted events against its share of the pooled ones, and reads the difference
  vector, every group but the last, through `numpy.linalg.pinv` of its covariance. Its weightings are
  `n`, `√n`, Peto's `∏ (1 − d/(n + 1))` including the current time, and `S^p (1 − S)^q` of the pooled
  **unweighted** Kaplan-Meier curve just before each time. The variance factor `(n − d)/(n − 1)` is one
  where infinite or undefined, and a negative one — a fractional risk set below one — makes the row's
  contribution `NaN`, then zero. `t_0` censors every event after it. Groups keep first-appearance order.
  Fleming-Harrington fails on a zero duration, the curve shorter than the table by a row.
- `survival_difference_at_fixed_point_in_time_test` reads each curve's estimate as a step and its
  Greenwood sum through `numpy.interp`, linearly, with the infinite term of a step where every subject
  left dies replaced by zero.
- `restricted_mean_survival_time` of a Kaplan-Meier fitter sums the mean exactly over the steps, and
  integrates the second moment by `scipy.integrate.quad` over the step function.
- `concordance_index` is Harrell's C with the ties `CoxSummary` already follows; a score reads as
  predicted survival.

## Decisions

As approved on the issue:

1. `LogRankOptions` (`Weighting`, `P`, `Q`, `Truncation`) and `LogRankWeighting` in
   `Lodestar.Abstractions`; `LogRank.Test` takes the options, and the weights beside them.
2. `LogRank.MultiGroup`, with and without subject weights, on `k − 1` degrees of freedom; the
   pseudo-inverse is a symmetric Jacobi eigendecomposition written here at numpy's `1e-15` cutoff, no
   published member offering one.
3. `LogRank.Pairwise`, returning `PairwiseLogRankResult` rows in lifelines' order.
4. `KaplanMeier.CompareAt`, returning `TestResult`; the Greenwood sum is rebuilt from the curve's steps.
5. `KaplanMeier.RestrictedMean`, returning `RestrictedMeanResult`.
6. `Concordance.Index`, the internal index Cox already used, made public.
7. The subject weights of `logrank_test` and `multivariate_logrank_test` are included; the pairwise
   test takes none, as lifelines' does not.
8. Proof by corpora, a random differential and a benchmark against lifelines.

## The one divergence

lifelines' `quad` over a step function lands up to 1.5 % relative from the exact second moment over
300 random curves. The variance here is the exact sum; `survival_restricted.json` freezes it at
`1e-9` and lifelines' figure beside it. The mean matches to the last digit.

## Proof

- `survival_logrank_family.json` (141 cases: every weighting, the truncation, whole, fractional and
  sub-one weights, three and four groups, a group censored before any event, the pairwise tables),
  `survival_restricted.json` (20) and `survival_concordance.json` (4), from lifelines 0.30.3, at `1e-9`.
- A random differential against lifelines, 1,904 cases under two seeds: every one matches, the lone
  failure being the suite's own check that lifelines' quadrature stays within 1 % of the exact
  variance, which one curve exceeded at 1.46 %; that bound is now 2 %.
- A benchmark against lifelines at 1,000, 10,000 and 100,000 subjects, ahead on every row: 4.42× on
  the pairwise tests at 100,000 at the least, 8,722× on the restricted mean at 1,000, where lifelines
  pays for its quadrature.
