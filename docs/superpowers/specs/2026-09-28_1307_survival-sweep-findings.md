# Lodestar.Survival findings of the invariant sweep after #1306

**Issues:** [#1307](https://github.com/CyrilB1531/lodestar/issues/1307),
[#1308](https://github.com/CyrilB1531/lodestar/issues/1308),
[#1309](https://github.com/CyrilB1531/lodestar/issues/1309).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The first Review B of `Lodestar.Survival` run as an invariant sweep (spec
`2026-09-28_1305_review-b-invariants.md`) found two violations and its final open review a third:

1. `ParametricOptions` copied `Breakpoints` in its `init` and kept the generated equality, which
   compares that copy by reference, so no two option sets were ever equal; its getter handed out the
   copy, so a caller could write an invalid breakpoint after the validation (#1307).
2. `CoxSummary`, `AftSummary` and `AalenSummary` sized a prediction as subjects × times in unchecked
   `int`, which wraps past `int.MaxValue` into an undocumented exception (#1308).
3. `PiecewiseExponential` gave an event on a breakpoint the left piece's hazard, `1/λ_j`; lifelines
   differentiates `min(breakpoint, t)` with autograd, which splits the tie, so its hazard there is
   `0.5/λ_j + 0.5/λ_{j+1}` (#1309). With breakpoints `[2, 5]` and durations on them, lifelines
   0.30.3 reports `hazard_at_times([2.0]) = 0.19107 = 0.5/λ0 + 0.5/λ1`.

## Decisions

- **`ParametricOptions` writes its own equality**, as every record with an array member does, over
  the three backing fields, which become `readonly` (S2328); its getter returns a copy, since
  `Breakpoints` is read once per fit.
- **One helper, `ResultTable.Length`**, takes the product in `long` and refuses past
  `Array.MaxLength` with `ArgumentException`, documented where it can surface. Swept as a class, not
  per reported site: the log-rank family's `rows × groups` and `k × k` tables and Aalen's
  `subjects × columns` design, on the fit and the prediction side, go through it too.
- **Align the tie, not document it.** A time equal to a breakpoint takes the mean of the two rates
  in `LogHazard`, which both the likelihood and `ParametricFit.Hazard` read.

## Verification

- `survival_parametric.json` gains one case appended last, eighteen integer durations of which seven
  sit on the breakpoints; every existing case is unchanged. Against the left-piece rule that case
  fails, and with the mean it passes.
- `SurvivalSweepTests` pins the option equality, the copy-on-read and the refused table size.
