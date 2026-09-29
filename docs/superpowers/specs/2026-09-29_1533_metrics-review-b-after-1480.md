# The Metrics findings of the Review B after #1480

**Issues:** [#1533](https://github.com/CyrilB1531/lodestar/issues/1533) to
[#1544](https://github.com/CyrilB1531/lodestar/issues/1544), and the six the Review B after #1272
left open: [#1273](https://github.com/CyrilB1531/lodestar/issues/1273),
[#1275](https://github.com/CyrilB1531/lodestar/issues/1275),
[#1277](https://github.com/CyrilB1531/lodestar/issues/1277),
[#1278](https://github.com/CyrilB1531/lodestar/issues/1278),
[#1280](https://github.com/CyrilB1531/lodestar/issues/1280),
[#1281](https://github.com/CyrilB1531/lodestar/issues/1281).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of `Lodestar.Metrics` after #1480, on `main` at `1cf61032`, found twelve defects
against scikit-learn 1.9.1, and six older ones were still open. They fall into four groups.

- **Weights.** Custom output weights on one output, sample weights summing to zero, and a weighted
  average over positives that vanish.
- **Labels.** A third label, a pair without `posLabel`, and one class in binary ROC AUC.
- **Bounds and precision.** An overflowing beta, an unbounded `nBins`, a label union, a sparse
  label table, and a drifting log-factorial.
- **Documentation and tests.** The pages still pointed at deleted rules, missed #1480's refusals and
  misstated ratios, and four tests pinned the wrong thing.

## Decisions

- **Weight rules are per metric.** `Outputs.Validate` takes the refusals a metric shares with its
  reference: every metric refuses output weights on one output except `root_mean_squared_error`,
  and zero-sum sample weights except `median_absolute_error`, as measured.
- **`posLabel` reads as scikit-learn's explicit `pos_label`.** The curves, the calibration and the
  Brier score accept one naming no present label and score every sample negative, as the reference
  does. `average_precision_score` refuses it. `roc_auc_score` and `log_loss` take none and infer the
  greater label, which a `posLabel` that always has a value cannot, so those refuse, with
  `PrfCounts`' sentence.
- **Binary ROC AUC answers `NaN`** for one class or a class weighted zero, as 1.9.1 does after its
  warning. The multiclass path's per-class score follows, which is also the reference's answer for
  an absent class under one-vs-rest.
- **`LabelIndex` bounds its direct table by the input**, `4·(labels + samples) + 1024`, not by the
  labels alone. The issue's `4·labels + 1024` made ten spaced labels over 10⁵ samples ten times
  slower. Two labels 4·10⁶ apart went from 15.6 MB and 4 ms a call to 0.4 KB and 0.5 µs.

## Rejected

- **Fixing `MedianAbsoluteError` on negative weights here.** It scores 0.5 where scikit-learn gives
  1.0 on `[1, -1, 0]`. Its percentile walk needs replaying against a non-monotonic cumulative
  weight first; that is [#1546](https://github.com/CyrilB1531/lodestar/issues/1546).
