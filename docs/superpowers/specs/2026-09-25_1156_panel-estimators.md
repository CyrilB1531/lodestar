# Panel estimators: fixed effects, between, first difference and random effects, at linearmodels parity

**Issue:** [#1156](https://github.com/CyrilB1531/lodestar/issues/1156).
**Status:** written before the work, 2026-09-25.
**Date:** 2026-09-25.

## The problem

`docs/equivalence.md` carries `PanelOLS(..., entity_effects=True, time_effects=True)`, `BetweenOLS`,
`FirstDifferenceOLS` and `RandomEffects` from `linearmodels` as *no counterpart yet*. No .NET
library estimates a panel model with its inference table
([decision 0004](../../decisions/0004-what-is-written-here-and-what-is-delegated.md)).

## Decisions

1. **One static class, four estimators.** `Lodestar.Stats.Regression.PanelRegression` with
   `FixedEffects` (`PanelOLS`: entity effects, time effects, both, or neither for pooled least
   squares), `Between`, `FirstDifference` and `RandomEffects` (Swamy and Arora's variance
   components). Each takes a `PanelDesign` and `PanelOptions`; a second overload takes one cluster
   label per row.
2. **The data types live in `Lodestar.Abstractions`**, namespace `Lodestar.Stats.Regression.Panel`:
   `PanelDesign` (a `readonly ref struct`: the response, the regressors, one entity and one time
   label per row, rows in any order), `PanelOptions`, `PanelCovarianceType` (`Unadjusted`, `Robust`,
   `Clustered`, `Kernel`) and `PanelSummary`. Forwarded from `Lodestar.Stats.Regression`, as the
   instrumental-variables types are.
3. **Shared with the instrumental variables, as Cyril decided**: `IvTest` becomes
   `Lodestar.Stats.Regression.WaldTest` and `IvKernel` becomes `Lodestar.Stats.Regression.KernelType`,
   both unpublished until now, so the rename breaks no caller.
4. **Defaults are the reference's**: an unadjusted covariance, **`Debiased = true`**, Bartlett's
   kernel with Driscoll and Kraay's default bandwidth `⌊4(T/100)^(2/9)⌋`, and an intercept
   (`WithIntercept`, which the reference leaves to the caller).
5. **Refused, as Cyril decided or as the reference refuses**: an intercept in first differences
   (`WithIntercept` must be set to `false` there); entity or time effects outside `FixedEffects`;
   a kernel covariance for `Between`; entity or time clusters for `Between`, time clusters for
   `FirstDifference`, and custom clusters that vary inside an entity (`Between`) or across a
   differenced pair (`FirstDifference`); more than two cluster dimensions; a Bartlett or Parzen
   bandwidth past the sample — which the instrumental-variables kernel now refuses too, where #1155
   read it as the sample.
6. **Behaviour replayed**, checked by an independent numpy reading against `linearmodels` 7.0 on
   random balanced and unbalanced panels before any C#:
   - two-way effects are the exact within transform: demeaned by the larger dimension, then purged
     of the other dimension's demeaned dummies (the reference's default algorithm); the grand means
     are added back when the regressors hold a constant;
   - the effects are counted in the degrees of freedom unless one effect is nested in the clusters,
     judged on the last cluster column, as the reference judges it;
   - the clustered covariance takes no `G/(G−1)` factor, and two-way clustering is `S₀ + S₁ − S₀₁`;
   - Driscoll-Kraay sums the scores by period before the kernel, scaled by `T/n`;
   - random effects: `σ²ₑ` from the within fit on `n − k − N + 1` degrees of freedom, `σ²ᵤ` from the
     between residuals less `σ²ₑ` over the harmonic mean of the group sizes, and `θᵢ` per entity;
   - the within, between and overall R², the homoskedastic and robust model tests, and the
     poolability F of the effects.
7. **Two readings differ from the reference.**
   - **`linearmodels` never sorts the rows**, and its first differences follow the order each
     period is first seen: when the first entity misses a period, it differences non-adjacent
     periods (`[0, 2, 1]` as the time axis, measured). Here rows are sorted by entity and period,
     and a first difference is taken between adjacent periods of the sorted grid. The two agree
     whenever the first entity observes every period, which the corpus keeps to.
   - p-values are the precise tail rather than `1 − cdf`, as for the instrumental variables.

## Proof

- `tests/oracles/stats_panel.json` from `linearmodels` 7.0: balanced and unbalanced panels, with
  and without an intercept, each estimator under each covariance it takes, one- and two-way
  effects, entity, time, two-way and custom clusters, each kernel at the default and a given
  bandwidth, debiased and not, compared at `1e-9` relative: 192 cases, 105 fixed effects, 21
  between, 27 first-difference and 39 random effects. A case whose covariance is past `1e6` in
  condition number, or has a non-positive variance (two-way clustering of a balanced panel's
  constant), is left out.
- A random differential run against `linearmodels`: 741 cases over 80 panels of 6 to 45 entities
  and 3 to 12 periods, 0 to 30% of the rows missing, every assertion within the same tolerance.

## Knock-on changes

Reference pages, `docs/equivalence.md` and `docs/migration/statsmodels.md` rows, samples, the
changelogs, and a benchmark against `linearmodels` in the package's `performance.md`, which also
records that no .NET incumbent exists.
