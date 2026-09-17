# 0850 — Metrics with fewer passes over their input

**Status:** accepted, 2026-09-17. Written after the measurement it records.

Issue: [#850](https://github.com/CyrilB1531/lodestar/issues/850), found by a performance review of `main`.

## Problem

Several metrics in `Lodestar.Metrics` paid for their generality on their common input: an unweighted
confusion matrix summed 1.0 per sample through a lookup that re-checked its table, a multilabel
matrix ran the general confusion matrix once per label or per row, the ranked-row scores allocated
their ordering per row, coverage error sorted each row to take a maximum, the curves gathered their
samples at random after the sort, one-vs-one ROC AUC scanned every sample three times per pair, and
the classification report re-read the same sums nine times.

## Change

- Unweighted confusion matrices count through the label offset table and read their totals off the
  cells; unweighted accuracy counts matches without a branch, in `Vector<int>` blocks on net10.
- Multilabel confusion matrices tally each 2 × 2 matrix in one pass.
- Expected mutual information tables its per-cell-count terms and hoists the marginals.
- ROC and precision-recall curves carry weight and class through the sort beside the key.
- One-vs-one ROC AUC groups the samples by class once.
- `Ndcg`, `Dcg` and `ReciprocalRank` reuse their row buffers; coverage error counts the scores at or
  above the lowest relevant one; label ranking average precision reuses its sort buffer.
- Cluster sizes use a direct table when the label range is small.
- The classification report reads true positives, predicted sums and supports once.

## Measured

| benchmark | `main` | fix |
| --- | ---: | ---: |
| `ConfusionMatrix.Compute`, 1,000,000 × 10 classes | 5.95 ms | **2.31 ms** |
| `Accuracy.Score`, 1,000,000 × 10 classes | 1.81 ms | **74.1 µs** |
| `MultilabelConfusionMatrix.Compute`, per class, 100,000 × 20 labels | 15.6 ms, 796 KB | **3.16 ms, 6.4 KB** |
| `RocCurve.Compute`, 1,000,000 samples | 137 ms, 99.3 MB | 100 ms, 53.8 MB |
| `RocAuc.MultiClass`, one-vs-one, 1,000,000 × 10 classes | 1.05 s, 0 B | 802 ms, 7.6 MB |
| `CoverageError.Score`, 100,000 × 10 labels | 27.6 ms, 10.7 MB | **3.73 ms, 782 KB** |
| `ClassificationReport.Compute`, 1,000 classes | 3.88 ms, 360 KB | **447 µs, 118 KB** |

The full table is in [the performance guide](../../guides/performance.md). Integer counts replace
sums of 1.0, exact below 2^53, and every other sum keeps its operands and order: bit-identical
against `main`. One-vs-one ROC AUC and the adjusted mutual information at 10 clusters allocate more
for their index and tables.

## Rejected

- **Skipping the tie pass in the tie-averaged gain.** It changes the last bits of the result.
- **Reusing `BinaryRoc`'s radix sort for the curves.** The radix sort is stable and `Array.Sort` is
  not, so tied samples would accumulate their weights in another order.
- **Label ranking loss by binary search.** It pays only past about 100 labels per row.
