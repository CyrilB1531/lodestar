# Lodestar.Survival's square tables of caller-sized counts

**Issues:** [#1311](https://github.com/CyrilB1531/lodestar/issues/1311).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The fix of #1308 bounded every table sized by two different caller counts and missed the tables sized by the
square of one: the information and covariance of Cox, time-varying Cox, the AFT models and Aalen's
additive model (covariates or parameters squared), the log-rank family's covariance and its
pseudo-inverse (groups squared), and Aalen's tables of event times, or columns, by columns. Past
46,341 the square wrapped in `int` into an undocumented exception. The sweep after #1310 found them
(spec `2026-09-28_1305_review-b-invariants.md`): an incomplete class fix, which is what the spec's
last rule is for.

## Decisions

- **Every such table goes through `ResultTable.Length`**, the Efron likelihood's included, which
  allocated its `featureCount²` with `checked` before the Newton loop was reached, naming the public parameter the count
  comes from (`featureCount` or `groups`), and each entry point's `ArgumentException` says so.
- **Products of the subjects by the covariates are left as they are**: they are bounded by the
  design's own length, which is already an array.

## Verification

- `ResultTable.Length` is pinned by `SurvivalSweepTests` (#1308); a grep of
  `src/Lodestar.Survival` for `new T[a * b]` outside it now finds only products bounded by the
  design.
