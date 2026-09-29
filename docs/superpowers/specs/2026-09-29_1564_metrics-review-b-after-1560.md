# The Metrics findings of the Review B after #1560

**Issues:** [#1564](https://github.com/CyrilB1531/lodestar/issues/1564),
[#1565](https://github.com/CyrilB1531/lodestar/issues/1565),
[#1566](https://github.com/CyrilB1531/lodestar/issues/1566),
[#1567](https://github.com/CyrilB1531/lodestar/issues/1567),
[#1568](https://github.com/CyrilB1531/lodestar/issues/1568).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of `Lodestar.Metrics` after #1560, on `main` at `ce017336`, against scikit-learn
1.9.1, found five defects:

- `MedianAbsoluteError` lost its finiteness check on output weights when #1560 gave it its own
  weight rules: `multioutput=[nan, 1]` scored NaN where scikit-learn raises.
- Binary `RocAuc` on one class validated the sample weights before answering NaN, so all-zero or
  NaN weights threw where `roc_auc_score` answers `nan` before `roc_curve` reads a weight.
- One-vs-one weighted over a single present class averaged no pairs and returned NaN, where
  scikit-learn raises `ZeroDivisionError`.
- Two log-factorial error figures were misstated.
- Five #1560 tests pinned only an exception type.

## Decisions

- **The median keeps two rules.** `OutputWeightsNeedOutputs | OutputWeightsFinite`: its percentile
  never divides by the weight total, so a zero sum still scores, but `check_array` still refuses a
  non-finite weight.
- **One class answers NaN before the weights.** The score is still checked for finiteness, as
  `check_array` checks it; a sample weight of any length or value is not read, and a score of
  another non-zero length is not compared, as scikit-learn reads and compares neither
  (`roc_auc_score([1,1,1], [.1,.2])` is `nan`). An empty score still fails, as `check_array`
  refuses it.
- **An empty weighting is refused where it is averaged.** `WeightedMean` refuses a zero weight
  total with numpy's sentence under `yTrue`. Writing the test found the parallel path failing
  earlier still: `RunPerIndex` built `ParallelOptions` with a degree of zero, so it now returns on
  an empty run.
- **Tests pin the refusal.** Where the reference's sentence is the product, the message; else the
  parameter. The NaN-score test pins today's per-class message: aligning it with `check_array`'s
  sentence makes the parallel per-pair failure path unreachable, so it is its own issue,
  [#1569](https://github.com/CyrilB1531/lodestar/issues/1569).
