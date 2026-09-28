# Table bounds in Lodestar.Preprocessing and TruncatedSvd after #1313

**Issues:** [#1314](https://github.com/CyrilB1531/lodestar/issues/1314),
[#1315](https://github.com/CyrilB1531/lodestar/issues/1315).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The invariant sweep after #1313 (spec `2026-09-28_1305_review-b-invariants.md`) found tables sized
by two caller counts in unchecked `int`: `OneHotEncoder.Transform`'s rows by encoded width, which a
million rows over 3,000 categories already wraps, `KBinsDiscretizer.Transform`'s rows by output
width, and the stratified splitters' groups or folds by classes (#1314). Its final open review
found `TruncatedSvd.Fit` accepting, since #1231, a component count whose range-finder block — the
longer side by `k + p` — passes the largest array, refused only inside `CsrMatrix` under a
`ParamName` `Fit` does not have (#1315).

## Decisions

- **One shared helper, `TableLength.Of`**, compiled with the other shared helpers, takes the product
  in `long` and refuses past `Array.MaxLength` with `ArgumentException` naming the public
  parameter. `Lodestar.Survival` keeps its own `ResultTable`, which #1308 introduced first.
- **Swept as a class after Review A** found five more sites: `PolynomialFeatures.Transform`, bounded
  by `int.MaxValue` rather than the largest array and blamed on `options`; the seeded stratified
  splitters, which reach the same allocation; `Nmf.Fit`'s initialisation, the same shape as #1315;
  and both `Transform`s' rows by components.
- **`TruncatedSvd` and `Nmf` refuse up front**, by `componentCount`, before Ω is drawn. scikit-learn makes
  the same allocation and fails with `MemoryError`; the allocation below that bound stays parity.

## Verification

- `TableBoundTests` refuses 43,000 rows over 50,000 categories before allocating, naming
  `values`; `TruncatedSvdTests` refuses a 1 × 50,000 matrix at 50,000 components naming
  `componentCount`, and `Nmf.Fit` refuses a 50,000 × 50,000 matrix at 45,000 components likewise.
