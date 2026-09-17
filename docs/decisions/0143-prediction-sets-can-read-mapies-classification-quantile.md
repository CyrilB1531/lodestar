---
status: accepted
supersedes: []
amends: ["0070"]
applies: []
---
# 0143 — Prediction sets can read MAPIE's classification quantile, and the ceiling rule stays the default

**Status:** accepted · **Date:** 2026-09-17 · **Amends:** [`0070`](0070-k-greater-than-n-returns-an-infinite-interval.md)

## Context

[Decision 0070](0070-k-greater-than-n-returns-an-infinite-interval.md) recorded that MAPIE 1.5.0
follows the ceiling rule, `k = ceil((n + 1)(1 - alpha))`, and not
`numpy.quantile(scores, (n + 1)(1 - alpha)/n, method="higher")`. That holds for
`SplitConformalRegressor`, and it is not what `SplitConformalClassifier` does:
`_compute_classification_quantile` in `mapie/utils.py` calls exactly that numpy quantile
([#866](https://github.com/CyrilB1531/lodestar/issues/866)).

The two rules read different order statistics. Measured against MAPIE 1.5.0 with a row whose
first class lies between the two thresholds, at `alpha = 0.1`:

| calibration size | ceiling rule reads | MAPIE's prediction set reads | the row's first class |
| --- | --- | --- | --- |
| 19 | the 18th smallest score | the 19th | MAPIE includes it, the ceiling rule does not |
| 99 | the 90th | the 91st | the same |

The three classification cases frozen for [#441](https://github.com/CyrilB1531/lodestar/issues/441)
sit where the rules agree, and their generator asserted MAPIE's sets against its own ceiling rule,
so the corpus could not see it.

## Decision

[`SplitConformal.Quantile`](../reference/conformal/prediction/splitconformal-quantile.md) takes a
[`ConformalQuantileRule`](../reference/conformal/prediction/conformalquantilerule.md).
`ConformalQuantileRule.Ceiling` is the zero value and what the two-argument overload uses, so every
existing call keeps its answer. `ConformalQuantileRule.MapieClassification` reads numpy's `higher`
quantile at MAPIE's level, and a prediction set built from it matches `predict_set`. Where that
level passes 1, numpy raises and this returns `double.PositiveInfinity`, as 0070 decided for the
ceiling rule.

## Options refused

**Switch prediction sets to MAPIE's rule outright**, through a second quantile member. Parity by
default, and a silent change to every set a caller already computes, for a difference of one rank.
The maintainer chose to keep the established answer and make parity a named request.

**Record the divergence and change nothing.** The package's promise is MAPIE parity, and a caller
who needs the sets MAPIE produces would have no way to get them.

## Consequences

- `docs/equivalence.md`'s `SplitConformalClassifier` row is identical only at
  `ConformalQuantileRule.MapieClassification`, and its numpy row names the rule it now matches.
- The conformal corpus carries two cases where the rules disagree, freezes both quantiles, and its
  generator asserts that they differ, so a rule wired to the other fails.
- 0070's sentence that MAPIE matches the ceiling rule on every case measured is true of the
  regressor only.
