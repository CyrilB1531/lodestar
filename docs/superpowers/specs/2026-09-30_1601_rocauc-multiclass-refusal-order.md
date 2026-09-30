# RocAuc.MultiClass refuses in scikit-learn's order, and its page says so

**Issues:** [#1601](https://github.com/CyrilB1531/lodestar/issues/1601).
**Status:** written with the work, 2026-09-30.
**Date:** 2026-09-30.

## The problem

The Review B of `Lodestar.Metrics` after #1599 found the `RocAuc.MultiClass` page putting the
one-vs-rest sample-weight refusals before the shape rules, where the code checks them after, and
calling `ArgumentException` first, where the `ArgumentOutOfRangeException` for the worker count and
`classCount` comes before everything. Review A then found the code's own order was not scikit-learn's
either: it checked the averaging, the weight length, the one-vs-one weight and the labels before the
row sums, and the label count before their order, where `_multiclass_roc_auc_score` refuses the row
sums first, then the averaging, then the labels — unique, ordered, counted, covering `y_true` — then
the weights.

## Decisions

- **The code takes scikit-learn's order, and its sentences.** The row sums, then the averaging, then
  the labels in scikit-learn's four steps with its four messages, then the weights: none under
  one-vs-one, one per sample, and under one-vs-rest their validity after the zero-total shortcut.
  The row-sum, label, averaging and one-vs-one weight messages are scikit-learn's — only the flat
  span's length keeps this package's, having no counterpart — and numpy's "Weights sum to zero,
  can't be normalized" loses the trailing period this package had added, everywhere it is raised or
  quoted.
- **The page follows as an ordered list**, the argument-range refusals first, which have no
  scikit-learn counterpart.
- **The length of the flat score span stays early**: scikit-learn's two-dimensional array cannot
  disagree with its own shape, so the check that stands for it keeps its place among the array
  checks.
- **Micro under one-vs-rest is scored, not refused.** Review A found scikit-learn accepting
  `average='micro'` there, where this refused it: `_average_binary_score` ravels `label_binarize`'s
  matrix and the scores and repeats each weight across its classes, one binary score. `Binary` is
  refused first, as `validate_params` refuses it before any array is read, and `Micro` under
  one-vs-one at the averaging step, both in scikit-learn's words.
- **The curve's own refusals follow `roc_curve` and `auc`.** Micro made a case reachable that binary
  `RocAuc.Score` already reached: a negative weight turning the false-positive rate back, which
  `auc` refuses and `BinaryRoc` integrated anyway. Under a negative weight the samples are now
  ordered as `roc_curve` orders them — score descending, a tie's earlier sample first, as
  `argsort(…, stable=True, descending=True)` leaves it — and the turn is tested on `fps / fps[-1]`,
  the quotient `auc` reads, from the 0 `roc_curve` prepends. Negatives whose weights total zero or
  less — not only zero — are `NaN`, as `roc_curve` makes those rates, and positives too unless the
  rate turns back first. A one-vs-rest class absent from `yTrue` is `NaN` before its curve reads a
  weight, as `_binary_roc_auc_score` stops on a one-label column, and a class curve's refusal inside
  a worker is rethrown as the lowest class's, restoring the failure slots #1569 had removed.
  Randomized differentials against `roc_auc_score` agree on every value, `NaN` and refusal: 1,000
  binary cases and 900 one-vs-rest cases under negative weights, on one worker and on four.
- **The weights are checked where `roc_curve` checks them.** One-vs-rest checks a non-finite or
  all-zero weight only when some class's column holds both labels, since a one-label column is
  `nan` before its curve reads a weight; and a class curve's refusal is kept from a worker only
  under a negative weight, anything else escaping as the defect it would be.
