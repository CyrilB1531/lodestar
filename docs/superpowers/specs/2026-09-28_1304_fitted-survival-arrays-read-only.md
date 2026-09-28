# Fitted survival models hand out nothing their predictions read

**Issues:** [#1304](https://github.com/CyrilB1531/lodestar/issues/1304).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The invariant sweep of `Lodestar.Abstractions` after #1303 (spec
`2026-09-28_1305_review-b-invariants.md`) found `CoxSummary.Baselines` handing out the
`CoxBaseline` records its predictions interpolate in: writing
`summary.Baselines[0].CumulativeHazard[3] = 0` changed every later `PredictCumulativeHazard`,
`PredictSurvivalFunction`, `PredictPercentile`, `PredictMedian` and `PredictExpectation`.

Swept as a class across `Lodestar.Survival`, the same defect had three more instances: `CoxSummary`,
`AftSummary`, `AalenSummary` and `ParametricFit` assigned the arrays their predictions read —
coefficients, covariate means, event times, cumulative hazards, parameters — to
`IReadOnlyList<double>` members, so a cast back to `double[]` edited the fit. #1232 had closed that
cast for `Lodestar.Decomposition` and `Lodestar.Preprocessing`.

## Decisions

- **`CoxBaseline` keeps its arrays.** Changing them to `IReadOnlyList<double>` would change the
  signature of a type `Lodestar.Survival` forwards, and part it from `KaplanMeierCurve` and the other
  curves, which read positionally too. Its remarks say the arrays are the record's own.
- **`CoxSummary` predicts from a copy.** Its `Baselines` initialiser takes a private copy of each
  baseline's times and cumulative hazard, the two arrays a prediction reads; writing to a handed-out
  baseline changes what the list shows, never a prediction.
- **Every array a summary is built from is wrapped by `Array.AsReadOnly`**, #1232's mechanism,
  including the ones no prediction reads, so that no statistic of a fit can be edited through it.
- **Each stratum's baseline gets its own times.** `CoxReport.Breslow` handed one `times` array to
  every stratum, so writing one baseline's times wrote the others'; `Accumulated` now copies it.

## Verification

- `FittedImmutabilityTests` fills a handed-out baseline's arrays and checks the prediction is
  unchanged, and checks that no list of the four fitted types is a `double[]`. Against `main` the
  first test would fail, since the prediction read the array it wrote.
