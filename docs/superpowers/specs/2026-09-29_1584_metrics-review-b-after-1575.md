# The Metrics findings of the Review B after #1575

**Issues:** [#1584](https://github.com/CyrilB1531/lodestar/issues/1584),
[#1585](https://github.com/CyrilB1531/lodestar/issues/1585),
[#1586](https://github.com/CyrilB1531/lodestar/issues/1586),
[#1587](https://github.com/CyrilB1531/lodestar/issues/1587),
[#1588](https://github.com/CyrilB1531/lodestar/issues/1588).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of `Lodestar.Metrics` after #1575, on `main` at `727318c7`, against scikit-learn
1.9.1, found five defects:

- `RocAuc.Score`'s XML and page still listed a length mismatch and a non-finite weight as refused,
  which a one-class `yTrue` now answers with NaN.
- No scores at all were refused with a length message; `check_array` refuses them first with
  "Found array with 0 sample(s) (shape=(0,)) while a minimum of 1 is required."
- Weighted one-vs-rest decided its zero-total shortcut on the sample sum; scikit-learn decides it on
  `average_weight.sum()`, the class totals summed. `[1e16, -1e16, 1, 0]` sums to 1 by sample and to
  0 by class, so this threw where scikit-learn returns 0.
- One-vs-one macro over one present class was untested on the parallel path.
- `MedianAbsoluteError`'s docs left out the zero-sum output-weight refusal.

## Decisions

- **The shortcut and the weighted mean sum as numpy does.** Each class's weight is summed in
  sample order, then the totals with numpy's `pairwise_sum`: in order below eight, eight running
  sums up to 128, halves past it. Review A found the order mattering from eight classes up: totals
  one `1e16`, six `1` and one `-1e16` sum to 0 in order and to 4 pairwise, where scikit-learn
  skips the shortcut and answers `nan`. `NumpyAverage.Mean` and `NumpyAverage.Weighted` follow `np.average` the same way,
  a zero-weighted score set to 0 first, and the class totals are computed once for both the
  shortcut and the weights. The one frozen bit pattern that moved, `multiclass_5` one-vs-one
  weighted, moved onto `roc_auc_score`'s own bits, `0x3FED6A6B9C6F6347`, two units in the last
  place from the sequential sum. The table as a whole is not scikit-learn's: the weighted
  one-vs-rest pins still differ from it in the per-class curve, which this change does not touch.
- **`check_array`'s refusals come first.** No labels, then no scores, then a non-finite score are
  refused in scikit-learn's words before `yTrue`'s labels or `posLabel` are read: `([], [.1])` and
  `([0,1,2], [])` get "Found array with 0 sample(s)…", `([0,1,2], [nan,1,2])` gets "Input
  contains NaN." where this said to use `MultiClass`. Two older tests that pinned this package's
  own wording for an empty input and an infinite score now pin scikit-learn's.
- **One helper, two metrics.** The pairwise sums live in `NumpyAverage` (`Sum`, `Mean`, `Weighted`),
  which `MultiClassRoc` and `AveragePrecision` both call: Review A found weighted multilabel average
  precision deciding the same shortcut on a running total across rows, the defect #1586 names,
  and dividing by it rather than by `np.average`'s weight sum. Its rows `(label 0, 1e16)`,
  `(label 1, -1e16)`, `(label 0, 1)` now score 0, as scikit-learn scores them.
- **One finiteness pass.** `RocAuc.Score` checks the scores before the labels, so the binary curve
  it calls skips its own per-score check on that path; the multiclass path keeps it, since a NaN
  passes the row sums there (#1569).
- **`RocAuc.MultiClass` speaks `check_array` too** for an empty input: `(shape=(0,))` under `yTrue`,
  `(shape=(0, k))` under `yScore`, before any length is compared.
- **A single column is binary.** scikit-learn reads an `(n, 1)` label matrix as a binary problem
  and scores it without the shortcut or the average, so `Averaging.Weighted` over one label returns
  that label's binary score exactly — `0.7862811791383221` for weights `[.7, 1.3, 2.9, .4]` on
  `[[1], [0], [1], [0]]` — and all-zero weights are refused, as the binary score refuses them.
