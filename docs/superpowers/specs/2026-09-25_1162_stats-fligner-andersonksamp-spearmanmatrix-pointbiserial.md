# Fligner-Killeen, the k-sample Anderson-Darling test, Spearman's matrix and the point-biserial correlation

**Issue:** [#1162](https://github.com/CyrilB1531/lodestar/issues/1162).
**Status:** written before the work, 2026-09-25.
**Date:** 2026-09-25.

## The problem

`docs/equivalence.md` carried `fligner`, `anderson_ksamp`, `spearmanr` on a 2-D array and
`pointbiserialr` as *no counterpart*, beside `weightedtau`, `somersd` and `binomtest`'s array mode.

## What scipy 1.18.1 does

- `fligner`: each group's absolute deviations from its centre (median, mean or trimmed mean),
  ranked together, turned into normal scores `Φ⁻¹(r/(2(N+1)) + 1/2)`; the statistic is the groups'
  weighted squared gaps from the pooled mean score over the scores' variance, read against
  `χ²(k − 1)`. An empty group answers `(nan, nan)`.
- `anderson_ksamp`: Scholz and Stephens' statistic under one of three variants, `midrank`
  (equation 7), `right` (6) and `continuous` (3); normalised by `σ_N`; its p-value is
  `exp(polyval(polyfit(critical, log(levels), 2), A²))`, clamped to `[0.001, 0.25]` outside the
  table. scipy 1.18 replaces `midrank` by `variant` and returns the critical values only when
  `variant` is unset.
- `spearmanr(a)` on 2-D: `numpy.corrcoef` of the ranked columns. Under `propagate` a variable with
  a `NaN` has `NaN` in its row and column; under `omit`, where a `NaN` is there to omit, `mstats`
  drops rows pair by pair and writes the diagonal as `(1, 0)` whatever the alternative. `numpy.cov` multiplies by `1/(n − 1)`,
  which can round a variable against itself one ulp below 1, and its p-value is then not zero.
- `pointbiserialr`: `pearsonr` on `x` as 0 and 1, two-sided only; below two pairs its wrapper
  answers `(nan, nan)` before `pearsonr` could raise.

## Decisions

As approved on the issue:

1. `Fligner.Test`, shaped as `Levene.Test`, returning a `TestResult`.
2. `AndersonDarling.KSample(variant, samples)`, returning the existing `AndersonResult` with the
   critical values kept; `AndersonKSampleVariant { Midrank, Right, Continuous }` in
   `Lodestar.Abstractions`; `method=PermutationMethod` left out, its draws being random.
3. `Spearman.Matrix(data, variableCount, alternative, nanPolicy)` returning `CorrelationMatrix`, a
   new record in `Lodestar.Abstractions` with both matrices row-major; always a matrix, two
   variables included.
4. `PointBiserial.Test(bool x, double y, nanPolicy)`, returning a `TestResult`.
5. `weightedtau` and `somersd` stay unwritten: no caller.
6. `binomtest`'s array mode stays out.
7. `Lodestar.Stats` reaches `Lodestar.Abstractions` by project until its next publication.

## Proof

- `stats_fligner.json` (23 cases), `stats_anderson_ksamp.json` (24), `stats_spearman_matrix.json`
  (15) and `stats_pointbiserial.json` (8), from scipy 1.18.1, compared at `1e-9`.
- A random differential against scipy: 250 cases of each family under two seeds, 2,000 in all.
  It is what found the diagonal's ulp, which the frozen corpus happened never to hit.
- A benchmark against scipy, ahead on every row. The first reading was behind on nine: each
  column was ranked once per partner, the k-sample test sorted with LINQ, and every normal score
  was refined by Newton. Ranking once, merge walks, AS 241 alone and a radix sort from 8,192
  values up — which every rank test in the package now uses — took them ahead.
