# The Metrics findings of the Review B after #1398

**Issues:** [#1461](https://github.com/CyrilB1531/lodestar/issues/1461) to
[#1479](https://github.com/CyrilB1531/lodestar/issues/1479).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of `Lodestar.Metrics` after #1398, on `main` at `97d2edfb`, swept the package invariant
by invariant against scikit-learn 1.9.1. It found three kinds of defect:

- **Inputs handled differently from scikit-learn.** A NaN or infinite `outputWeights`, Tweedie
  `power` or top-k score was accepted, and so was a single-label `log_loss`. A NaN calibration
  probability and `beta = inf` were refused, where scikit-learn accepts both.
- **Six allocations or shape checks** multiplied caller sizes in `int`.
- **Live arrays.** The curves exposed theirs without saying so.

On top of those, #1396 had repointed the package's citations of deleted decision records to the
commit that still holds them, and the pages still named rules whose records are gone.

## Decisions

- **Align with the reference rather than document the divergence.** Every refusal or acceptance
  above now follows scikit-learn. Each case was measured on 1.9.1, and the tests pin the values:
  - NaN, `inf` and the single-label `yTrue` are refused where scikit-learn refuses them;
  - `beta = inf` returns the recall;
  - a NaN probability is binned last, and with a NaN present an infinity passes too, because
    numpy's extremes turn NaN; under the quantile strategy every edge is NaN.
- **`LogLoss.Score` has no `labels`.** It refuses a single label, and its message points to
  `MultiClass`, whose `classCount` names every class as `labels` does.
- **`TopKAccuracy`'s NaN path is gone.** A NaN no longer reaches the ranking, so the sort it fell
  back to, and the `Ranking.Descending` overload only it called, are removed.
- **Bounds.** A square table goes through `TableLength.Of`, a pair count and the label union are
  summed in `long`, and a shape check multiplies in `long`.
- **The curves wrap their arrays** with `Array.AsReadOnly`, as `ConfusionMatrix.Labels` already did.
- **Citations of deleted records are removed**, not repointed, and each named rule is replaced by
  the clause it stood for.

## Measured

`TopKAccuracyBenchmarks.TopTwo` is the one hot loop the fixes touch: a finiteness check over the
scores was added, and the NaN test removed from the inner loop. It was measured A/B/A on an AMD
Ryzen 7 8700G, .NET 10.0.12 and BenchmarkDotNet 0.14.0, with the machine lock held. A is `main` at
`97d2edfb`.

| Classes | A | B | A again |
| --- | --- | --- | --- |
| 10 | 7.761 ms | 7.608 ms | 7.724 ms |
| 100 | 59.394 ms | 56.736 ms | 59.634 ms |

## Rejected

- **Editing the released CHANGELOG sections** that link older records: they are history.
